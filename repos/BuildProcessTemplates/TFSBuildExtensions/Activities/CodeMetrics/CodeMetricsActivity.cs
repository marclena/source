using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.IO;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using TFSBuildExtensions.Library.CodeMetrics.Report;
using System.Globalization;
using System.Diagnostics;
using TFSBuildExtensions.Library.CodeMetrics.Threshold;
using TFSBuildExtensions.Library.RepositoryContracts;
using Vueling.Activitites.DB.Infrastructure.ADORepositories;

namespace TFSBuildExtensions.CodeMetrics
{
    /// <summary>
    /// Activity for processing code metrics using the Visual Studio Code Metrics PowerTool 11.0
    /// (http://www.microsoft.com/en-us/download/details.aspx?id=38196)
    /// <para/>
    /// <example>
    /// <code lang="xml"><![CDATA[
    /// <!-- Run Code Metrics for SampleApp.exe and SampleLibrary.dll -->    
    /// <tac:CodeMetrics FailBuildOnError="{x:Null}" TreatWarningsAsErrors="{x:Null}" BinariesDirectory="[BinariesDirectory]" CyclomaticComplexityErrorTreshold="15" CyclomaticComplexityWarningTreshold="10" FilesToProcess="[New List(Of String)(New String() {&quot;SampleApp.exe&quot;, &quot;SampleLibrary.dll&quot;})]" GeneratedFileName="Metrics.xml" LogExceptionStack="True" MaintainabilityIndexErrorTreshold="40" MaintainabilityIndexWarningTreshold="90" />
    /// ]]></code>    
    /// </example>
    /// </summary>
    [BuildActivity(HostEnvironmentOption.All)]
    public class CodeMetricsActivity : BaseCodeActivity
    {
        private IRepositoryCodeMetricsResults repositoryCodeMetricsResults;
        private const string MaintainabilityIndex = "MaintainabilityIndex";
        private const string CyclomaticComplexity = "CyclomaticComplexity";
        private const string ClassCoupling = "ClassCoupling";
        private const string DepthOfInheritance = "DepthOfInheritance";
        private const string LinesOfCode = "LinesOfCode";

        /// <summary>
        /// Path to where the binaries are placed
        /// </summary>
        [Description("Path to where the binaries are placed")]
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        /// <summary>
        /// Which files that should be processed. Can be a list of files or file match patterns. Defaults to *.dll;*.exe
        /// </summary>        
        [Description("Which files that should be processed. Can be a list of files or file match patterns. Defaults to *.dll;*.exe")]
        [RequiredArgument]
        public InArgument<IEnumerable<string>> FilesToProcess { get; set; }

        /// <summary>
        /// Name of the output metrics result file. Should end with .xml
        /// </summary>
        [Description("Optional: Name of the output metrics result file.")]
        [RequiredArgument]
        public InArgument<string> OutputFile { get; set; }

        /// <summary>
        /// Name of the threshold metrics result file. Should end with .xml
        /// </summary>
        [Description("Optional: Name of the threshold metrics result file.")]
        [RequiredArgument]
        public InArgument<string> ThresholdFile { get; set; }        

        /// <summary>
        /// Executes the logic for this workflow activity
        /// </summary>
        protected override void InternalExecute()
        {
            this.InformationMessageList = new List<string>();
            this.WarningMessageList = new List<string>();
            this.ErrorMessageList = new List<string>();

            string generatedFile = null;
            if (this.OutputFile != null && !string.IsNullOrEmpty(this.OutputFile.Get(this.ActivityContext)))
            {
                generatedFile = this.OutputFile.Get(this.ActivityContext);
            }
            else
                return;

            if (!this.RunCodeMetrics(generatedFile))
            {
                return;
            }

            string fileName = Path.GetFileName(generatedFile);
            string pathToFileInDropFolder = Path.Combine((String.IsNullOrEmpty(this.BuildDetail.DropLocation) ? generatedFile : this.BuildDetail.DropLocation), fileName);

            if (this.ActivityContext.GetExtension<IBuildLoggingExtension>() != null)
            {
                IActivityTracking currentTracking =
                    this.ActivityContext.GetExtension<IBuildLoggingExtension>().GetActivityTracking(this.ActivityContext);
                IBuildInformationNode rootNode = AddTextNode("Processing metrics", currentTracking.Node);

                AddLinkNode(fileName, new Uri(pathToFileInDropFolder), rootNode);
            }

            CodeMetricsReport result = CodeMetricsReport.LoadFromFile(generatedFile);
            if (result == null)
            {
                LogBuildMessage("Could not load metric result file ");
                return;
            }

            CodeMetricsThreshold codeMetricsThreshold = CodeMetricsThreshold.LoadFromFile(this.ThresholdFile.Get(this.ActivityContext));
            if (result == null)
            {
                LogBuildMessage("Could not load metric result file ");
                return;
            }

            Build build = new Build();
            build.Name = Path.GetFileNameWithoutExtension(pathToFileInDropFolder);
            CreateInitialBuildMetrics(build);

            try
            {
                repositoryCodeMetricsResults = new RepositoryCodeMetricsResults();

                foreach (Target target in result.Targets)
                {
                    foreach (Module module in target.Modules)
                    {
                        //Add metrics to average build
                        AddAssemblyMetricsToBuildMetricsAverage(build, module.Metrics);
                        //Save application Metrics to database
                        repositoryCodeMetricsResults.addApplicationCodeMetricsResult(module, build.Name);

                        this.ProcessMetrics(module.Name, module.Metrics, codeMetricsThreshold.Assembly);
                        foreach (Namespace ns in module.Namespaces)
                        {
                            this.ProcessMetrics(String.Format("{0} in {1}", ns.Name, module.Name), ns.Metrics, codeMetricsThreshold.Namespace);
                            foreach (TFSBuildExtensions.Library.CodeMetrics.Report.Type type in ns.Types)
                            {
                                this.ProcessMetrics(String.Format("{0}.{1} in {2}", ns.Name, type.Name, module.Name), type.Metrics, codeMetricsThreshold.Type);
                                foreach (Member member in type.Members)
                                {
                                    this.ProcessMetrics(String.Format("{0}.{1}.{2} in {3}", ns.Name, type.Name, member.Name, module.Name), member.Metrics, codeMetricsThreshold.Member);
                                }
                            }
                        }
                    }
                }

                SetAverageMetricsToBuild(build, result.Targets.Count());
                result.Build = build;
                SaveMetricsToDatabase(build);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (this.ErrorMessageList.Count() > 0)
                {
                    throw new Exception("Error processing metrics.");
                }
            }
        }


        private void CreateInitialBuildMetrics(Build build)
        {
            build.Metrics.Add(new Metric("MaintainabilityIndex", "0"));
            build.Metrics.Add(new Metric("CyclomaticComplexity", "0"));
            build.Metrics.Add(new Metric("ClassCoupling", "0"));
            build.Metrics.Add(new Metric("DepthOfInheritance", "0"));
            build.Metrics.Add(new Metric("LinesOfCode", "0"));
        }

        private void SaveMetricsToDatabase(Build build)
        {
            repositoryCodeMetricsResults.addBuildCodeMetricsResult(build);
        }

        private void SetAverageMetricsToBuild(Build build, int count)
        {
            foreach (Metric metric in build.Metrics)
            {
                if (!metric.Name.Equals("LinesOfCode"))
                {
                    metric.Value = (decimal.Parse(metric.Value)/count).ToString();
                }
            }
        }
        private void AddAssemblyMetricsToBuildMetricsAverage(Build build, List<Metric> metrics)
        {
            foreach (var metric in metrics)
            {
                if (metric != null && !string.IsNullOrEmpty(metric.Value))
                {
                    foreach (var buildMetric in build.Metrics)
                    {
                        if (metric.Name.Equals(buildMetric.Name))
                        {
                            buildMetric.Value = (decimal.Parse(buildMetric.Value) + decimal.Parse(metric.Value)).ToString();
                            break;
                        }
                            
                    }
                }
            }        
        }

        /// <summary>
        /// Override for base.CacheMetadata
        /// </summary>
        /// <param name="metadata">CodeActivityMetadata</param>
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.RequireExtension(typeof(IBuildDetail));
        }

        private bool RunCodeMetrics(string output)
        {
            string metricsExePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\Team Tools\Static Analysis Tools\FxCop\metrics.exe");
            if (!File.Exists(metricsExePath))
            {
                LogBuildError("Could not locate " + metricsExePath + ". Please download Visual Studio Code Metrics PowerTool 11.0 at http://www.microsoft.com/en-us/download/details.aspx?id=38196");
                return false;
            }

            if (this.FilesToProcess.Get(this.ActivityContext) == null || this.FilesToProcess.Get(this.ActivityContext).Count() == 0)
            {
                this.FilesToProcess.Set(this.ActivityContext, new List<string> { "*.dll", "*.exe" });
            }

            //Get files to process with associated pdb file
            List<string> files = this.GetFiles(this.FilesToProcess.Get(this.ActivityContext).ToList());

            if (files.Count == 0)
            {
                this.LogBuildMessage("No targets were selected.");
                return false;
            }

            string metricsExeArguments = files.Aggregate(string.Empty, (current, file) => current + string.Format(" /f:\"{0}\\{1}\"", this.BinariesDirectory.Get(this.ActivityContext), file));
            metricsExeArguments += string.Format(" /out:\"{0}\" /gac /iit /igc /assemblyCompareMode:StrongNameIgnoringVersion", output);

            ProcessStartInfo psi = new ProcessStartInfo { FileName = metricsExePath, UseShellExecute = false, RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true, Arguments = metricsExeArguments, WorkingDirectory = this.BinariesDirectory.Get(this.ActivityContext) };
            this.LogBuildMessage("Running " + psi.FileName + " " + psi.Arguments);

            using (Process process = Process.Start(psi))
            {
                using (ManualResetEvent mreOut = new ManualResetEvent(false), mreErr = new ManualResetEvent(false))
                {
                    process.OutputDataReceived += (o, e) =>
                    {
                        if (e.Data == null)
                        {
                            mreOut.Set();
                        }
                        else
                        {
                            this.LogBuildMessage(e.Data);
                        }
                    };
                    process.BeginOutputReadLine();
                    process.ErrorDataReceived += (o, e) =>
                    {
                        if (e.Data == null)
                        {
                            mreErr.Set();
                        }
                        else
                        {
                            this.LogBuildMessage(e.Data);
                        }
                    };
                    process.BeginErrorReadLine();
                    process.StandardInput.Close();
                    process.WaitForExit();

                    mreOut.WaitOne();
                    mreErr.WaitOne();

                    if (process.ExitCode != 0)
                    {
                        this.LogBuildError(process.ExitCode.ToString(CultureInfo.CurrentCulture));
                        return false;
                    }

                    if (!File.Exists(output))
                    {
                        this.LogBuildError("Could not locate file " + output);
                        return false;
                    }
                }
            }

            return true;
        }

        private List<string> GetFiles(List<string> patterns)
        {
            List<string> files = new List<string>();
            foreach (string pattern in patterns)
            {
                List<string> result = Directory.GetFiles(this.BinariesDirectory.Get(this.ActivityContext), (pattern.EndsWith(".dll")?pattern:pattern + ".dll")).ToList();
                foreach (string file in result)
                {
                    string pdbFile = String.Format("{0}.pdb", Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)));
                    if (File.Exists(pdbFile))
                        files.Add(Path.GetFileName(file + (file.EndsWith(".dll")?"":".dll")));
                }
            }

            return files.Distinct().ToList();
        }

        /// <summary>
        /// Analyzes the resulting metrics file and compares the Maintainability Index and Cyclomatic Complexity against the threshold values
        /// </summary>
        /// <param name="member">Name of the member (namespace, module, type...)</param>
        /// <param name="metrics">The metrics for this member</param>
        /// <param name="parent">The parent node in the build log</param>
        private void ProcessMetrics(string member, IEnumerable<Metric> metrics, LevelMetricsThreshold levelThreshold)
        {
            foreach (var metric in metrics)
            {
                int metricValue;
                if (metric != null && !string.IsNullOrEmpty(metric.Value) && int.TryParse(metric.Value, out metricValue))
                {
                    if (!String.IsNullOrEmpty(levelThreshold.MaintainabilityIndexError) && metric.Name == MaintainabilityIndex && Convert.ToInt32(metric.Value) < Convert.ToInt32(levelThreshold.MaintainabilityIndexError))
                    {
                        this.FailCurrentBuild();
                        this.ErrorMessageList.Add(string.Format("{0} for {1} is {2} which is below threshold ({3})", MaintainabilityIndex, member, metric.Value, Convert.ToInt32(levelThreshold.MaintainabilityIndexError)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.MaintainabilityIndexWarning) && metric.Name == MaintainabilityIndex && metricValue < Convert.ToInt32(levelThreshold.MaintainabilityIndexWarning))
                    {
                        this.PartiallyFailCurrentBuild();
                        LogBuildWarning(string.Format("{0} for {1} is {2} which is below threshold ({3})", MaintainabilityIndex, member, metric.Value, Convert.ToInt32(levelThreshold.MaintainabilityIndexWarning)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.CyclomaticComplexityError) && metric.Name == CyclomaticComplexity && Convert.ToInt32(metric.Value) > Convert.ToInt32(levelThreshold.CyclomaticComplexityError))
                    {
                        this.FailCurrentBuild();
                        this.ErrorMessageList.Add(string.Format("{0} for {1} is {2} which is above threshold ({3})", CyclomaticComplexity, member, metric.Value, Convert.ToInt32(levelThreshold.CyclomaticComplexityError)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.CyclomaticComplexityWarning) && metric.Name == CyclomaticComplexity && metricValue > Convert.ToInt32(levelThreshold.CyclomaticComplexityWarning))
                    {
                        this.PartiallyFailCurrentBuild();
                        LogBuildWarning(string.Format("{0} for {1} is {2} which is above threshold ({3})", CyclomaticComplexity, member, metric.Value, Convert.ToInt32(levelThreshold.CyclomaticComplexityWarning)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.ClassCouplingError) && metric.Name == ClassCoupling && Convert.ToInt32(metric.Value) > Convert.ToInt32(levelThreshold.ClassCouplingError))
                    {
                        this.FailCurrentBuild();
                        this.ErrorMessageList.Add(string.Format("{0} for {1} is {2} which is above threshold ({3})", ClassCoupling, member, metric.Value, Convert.ToInt32(levelThreshold.ClassCouplingError)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.ClassCouplingWarning) && metric.Name == ClassCoupling && metricValue > Convert.ToInt32(levelThreshold.ClassCouplingWarning))
                    {
                        this.PartiallyFailCurrentBuild();
                        LogBuildWarning(string.Format("{0} for {1} is {2} which is above threshold ({3})", ClassCoupling, member, metric.Value, Convert.ToInt32(levelThreshold.ClassCouplingWarning)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.DepthOfInheritanceError) && metric.Name == DepthOfInheritance && Convert.ToInt32(metric.Value) > Convert.ToInt32(levelThreshold.DepthOfInheritanceError))
                    {
                        this.FailCurrentBuild();
                        this.ErrorMessageList.Add(string.Format("{0} for {1} is {2} which is above threshold ({3})", DepthOfInheritance, member, metric.Value, Convert.ToInt32(levelThreshold.DepthOfInheritanceError)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.DepthOfInheritanceWarning) && metric.Name == DepthOfInheritance && metricValue > Convert.ToInt32(levelThreshold.DepthOfInheritanceWarning))
                    {
                        this.PartiallyFailCurrentBuild();
                        LogBuildWarning(string.Format("{0} for {1} is {2} which is above threshold ({3})", DepthOfInheritance, member, metric.Value, Convert.ToInt32(levelThreshold.DepthOfInheritanceWarning)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.LinesOfCodeError) && metric.Name == LinesOfCode && Convert.ToInt32(metric.Value) > Convert.ToInt32(levelThreshold.LinesOfCodeError))
                    {
                        this.FailCurrentBuild();
                        this.ErrorMessageList.Add(string.Format("{0} for {1} is {2} which is above threshold ({3})", LinesOfCode, member, metric.Value, Convert.ToInt32(levelThreshold.LinesOfCodeError)));
                    }

                    if (!String.IsNullOrEmpty(levelThreshold.LinesOfCodeWarning) && metric.Name == LinesOfCode && metricValue > Convert.ToInt32(levelThreshold.LinesOfCodeWarning))
                    {
                        this.PartiallyFailCurrentBuild();
                        LogBuildWarning(string.Format("{0} for {1} is {2} which is above threshold ({3})", LinesOfCode, member, metric.Value, Convert.ToInt32(levelThreshold.LinesOfCodeWarning)));
                    }
                }
            }
        }

        private void PartiallyFailCurrentBuild()
        {
            this.BuildDetail.Status = BuildStatus.PartiallySucceeded;
            this.BuildDetail.Save();
        }

        private void FailCurrentBuild()
        {
            this.BuildDetail.Status = BuildStatus.Failed;
            this.BuildDetail.Save();
        }

        private bool IsErrorIgnored(int code, string errorMessage)
        {
            return code == 9 && errorMessage.Length > 0 && errorMessage.Contains("No targets were selected");
        }
    }
}