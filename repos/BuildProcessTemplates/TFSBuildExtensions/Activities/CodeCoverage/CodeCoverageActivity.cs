using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.IO;
using TFSBuildExtensions.Library.CodeCoverage.Threshold;
using TFSBuildExtensions.Library.CodeCoverage.Report;
using System.Diagnostics;
using TFSBuildExtensions.Library.RepositoryContracts;
using Vueling.Activitites.DB.Infrastructure.ADORepositories;

namespace TFSBuildExtensions.CodeCoverage
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class CodeCoverageActivity : BaseCodeActivity
    {
        #region private properties and constants

        private const string defaultOutlineFilename = "CodeCoverageResults.xml";
        private const string CodeCoverageIndex = "Code Coverage";

        private InArgument<decimal> buildCodeCoveragePercentageError = 0;
        private InArgument<decimal> buildCodeCoveragePercentageWarning = 0;
        private InArgument<decimal> assemblyCodeCoveragePercentageError = 0;
        private InArgument<decimal> assemblyCodeCoveragePercentageWarning = 0;

        private OutArgument<TFSBuildExtensions.Library.CodeCoverage.Enums.Response> response;
        private IRepositoryCodeCoverageResults iRepositoryCodeCoverageResults;

        #endregion

        #region properties

        [Description("Sources Directory")]
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [Description("Binaries directory")]
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        [Description("Test root path")]
        [RequiredArgument]
        public InArgument<string> TestRootPath { get; set; }

        [Description("folder base")]
        [RequiredArgument]
        public InArgument<string> folderBase { get; set; }

        /// <summary>
        /// Name of the threshold code coverage result file. Should end with .xml
        /// </summary>
        [Description("Optional: Name of the threshold code coverage result file.")]
        public InArgument<string> ThresholdFile { get; set; }

        [Description("BuildCodeCoveragePercentageError value")]
        public InArgument<decimal> BuildCodeCoveragePercentageError
        {
            get { return this.buildCodeCoveragePercentageError; }
            set { this.buildCodeCoveragePercentageError = value; }
        }

        [Description("BuildCodeCoveragePercentageWarning value")]
        public InArgument<decimal> BuildCodeCoveragePercentageWarning
        {
            get { return this.buildCodeCoveragePercentageWarning; }
            set { this.buildCodeCoveragePercentageWarning = value; }
        }

        [Description("AssemblyCodeCoveragePercentageError value")]
        public InArgument<decimal> AssemblyCodeCoveragePercentageError
        {
            get { return this.assemblyCodeCoveragePercentageError; }
            set { this.assemblyCodeCoveragePercentageError = value; }
        }

        [Description("AssemblyCodeCoveragePercentageWarning value")]
        public InArgument<decimal> AssemblyCodeCoveragePercentageWarning
        {
            get { return this.assemblyCodeCoveragePercentageWarning; }
            set { this.assemblyCodeCoveragePercentageWarning = value; }
        }

        [Description("Response")]
        public OutArgument<TFSBuildExtensions.Library.CodeCoverage.Enums.Response> ResponseCodeCoverage
        {
            get { return this.response; }
            set { this.response = value; }
        }

        #endregion

        #region private properties

        string _codeCoverageXml;
        string _dataCoverageFile;

        #endregion

        protected override void InternalExecute()
        {
            this.BuildDetail = this.ActivityContext.GetExtension<IBuildDetail>();
            this.response.Set(this.ActivityContext, TFSBuildExtensions.Library.CodeCoverage.Enums.Response.Ok);
            _codeCoverageXml = this.BinariesDirectory.Get(this.ActivityContext) + @"\" + defaultOutlineFilename;
            
            _dataCoverageFile = this.TestRootPath.Get(this.ActivityContext) + @"\" + GetPathTestRun() + @"\In\" + System.Environment.MachineName + @"\data.coverage";
            if (!File.Exists(_dataCoverageFile))
            {
                LogBuildWarning("File data.coverage does not exist.");
                this.response.Set(this.ActivityContext, Library.CodeCoverage.Enums.Response.DataCoverageFileNotFound);
                return;
            }

            codeCoverageConverter();
        }

        private void codeCoverageConverter()
        {
            CodeCoverageThreshold codeCoverageThreshold;
            RunCodeCoverage();


            if (this.response.Get(this.ActivityContext) == Library.CodeCoverage.Enums.Response.CodeCoverageConsoleNotFound)
            {
                return;
            }
            CoverageDSPriv result = CoverageDSPriv.LoadFromFile(_codeCoverageXml);
            if (result == null)
            {
                LogBuildMessage("Could not load code coverage xml result file. ");
                this.response.Set(this.ActivityContext, Library.CodeCoverage.Enums.Response.DataCoverageXmlFileLoadError);
                return;
            }

            if (File.Exists(this.ThresholdFile.Get(this.ActivityContext)))
            {
                codeCoverageThreshold = CodeCoverageThreshold.LoadFromFile(this.ThresholdFile.Get(this.ActivityContext));
            }
            else
            {
                codeCoverageThreshold = new CodeCoverageThreshold();
                codeCoverageThreshold.Build = new LevelCoverageThreshold();
                codeCoverageThreshold.Assembly = new LevelCoverageThreshold();
                codeCoverageThreshold.Build.CodeCoveragePercentageError = this.buildCodeCoveragePercentageError.Get(this.ActivityContext).ToString();
                codeCoverageThreshold.Build.CodeCoveragePercentageWarning = this.buildCodeCoveragePercentageWarning.Get(this.ActivityContext).ToString();
                codeCoverageThreshold.Assembly.CodeCoveragePercentageError = this.assemblyCodeCoveragePercentageError.Get(this.ActivityContext).ToString();
                codeCoverageThreshold.Assembly.CodeCoveragePercentageWarning = this.assemblyCodeCoveragePercentageWarning.Get(this.ActivityContext).ToString();
            }

            ProcessCoverage(result, codeCoverageThreshold);
        }

        private void ProcessCoverage(TFSBuildExtensions.Library.CodeCoverage.Report.CoverageDSPriv result, TFSBuildExtensions.Library.CodeCoverage.Threshold.CodeCoverageThreshold codeCoverageThreshold)
        {
            iRepositoryCodeCoverageResults = new RepositoryCodeCoverageResults();

            iRepositoryCodeCoverageResults.addBuildCodeCoverageResult(this.BuildDetail.BuildNumber, this.BuildDetail.BuildDefinition.Name, Decimal.Round(result.BuildField.BuildCodeCoveragePercentage, 5));

            if (!String.IsNullOrEmpty(codeCoverageThreshold.Build.CodeCoveragePercentageError) && result.BuildField.BuildCodeCoveragePercentage < Decimal.Parse(codeCoverageThreshold.Build.CodeCoveragePercentageError))
            {
                this.response.Set(this.ActivityContext, TFSBuildExtensions.Library.CodeCoverage.Enums.Response.BuildCodeCoverageError);
                this.FailCurrentBuild();
                LogBuildError(string.Format("{0} for {1} is {2} which is below threshold ({3})", CodeCoverageIndex, "entire Build", result.BuildField.BuildCodeCoveragePercentage, Convert.ToInt32(codeCoverageThreshold.Build.CodeCoveragePercentageError)));
            }

            if (!String.IsNullOrEmpty(codeCoverageThreshold.Build.CodeCoveragePercentageWarning) && result.BuildField.BuildCodeCoveragePercentage < Decimal.Parse(codeCoverageThreshold.Build.CodeCoveragePercentageWarning))
            {
                this.response.Set(this.ActivityContext, TFSBuildExtensions.Library.CodeCoverage.Enums.Response.BuildCodeCoverageWarning);
                LogBuildWarning(string.Format("{0} for {1} is {2} which is below threshold ({3})", CodeCoverageIndex, "entire Build", result.BuildField.BuildCodeCoveragePercentage, Convert.ToInt32(codeCoverageThreshold.Build.CodeCoveragePercentageWarning)));
            }

            foreach (CoverageDSPrivModule coverageDSPrivModule in result.Module)
            {
                decimal percentageCodeCoverage = (((decimal)coverageDSPrivModule.BlocksCovered / ((decimal)coverageDSPrivModule.BlocksCovered + (decimal)coverageDSPrivModule.BlocksNotCovered)) * 100);

                iRepositoryCodeCoverageResults.addApplicationCodeCoverageResult(this.BuildDetail.BuildNumber, coverageDSPrivModule.ModuleName.Substring(0, coverageDSPrivModule.ModuleName.Length-4), Decimal.Round(percentageCodeCoverage, 5));

                if (!String.IsNullOrEmpty(codeCoverageThreshold.Assembly.CodeCoveragePercentageError) && percentageCodeCoverage < Decimal.Parse(codeCoverageThreshold.Assembly.CodeCoveragePercentageError))
                {
                    this.response.Set(this.ActivityContext, TFSBuildExtensions.Library.CodeCoverage.Enums.Response.AssemblyCodeCoverageError);
                    this.FailCurrentBuild();
                    LogBuildError(string.Format("{0} for {1} is {2} which is below threshold ({3})", CodeCoverageIndex, coverageDSPrivModule.ModuleName, percentageCodeCoverage, Convert.ToInt32(codeCoverageThreshold.Assembly.CodeCoveragePercentageError)));
                }

                if (!String.IsNullOrEmpty(codeCoverageThreshold.Assembly.CodeCoveragePercentageWarning) && percentageCodeCoverage < Decimal.Parse(codeCoverageThreshold.Assembly.CodeCoveragePercentageWarning))
                {
                    this.response.Set(this.ActivityContext, TFSBuildExtensions.Library.CodeCoverage.Enums.Response.AssemblyCodeCoverageWarning);
                    LogBuildWarning(string.Format("{0} for {1} is {2} which is below threshold ({3})", CodeCoverageIndex, coverageDSPrivModule.ModuleName, percentageCodeCoverage, Convert.ToInt32(codeCoverageThreshold.Assembly.CodeCoveragePercentageWarning)));
                }
            }
        }

        private void RunCodeCoverage()
        {
            LogBuildMessage(string.Format("Trying to get code coverage information from {0}", _dataCoverageFile), BuildMessageImportance.Normal);

            string coverageExePath = this.folderBase.Get(this.ActivityContext) + @"\Vueling.Build.CodeCoverage.ConsoleUI\Vueling.Build.CodeCoverage.ConsoleUI.exe";
            if (!File.Exists(coverageExePath))
            {
                LogBuildError("Could not locate " + coverageExePath + ". Please verify workspace in this Build Definition.");
                this.response.Set(this.ActivityContext, Library.CodeCoverage.Enums.Response.CodeCoverageConsoleNotFound);
                return;
            }

            string outTestDirectory = this.TestRootPath.Get(this.ActivityContext) + @"\" + GetPathTestRun() + @"\Out";

            string coverageExeArguments = string.Format(" /INFILE:\"{0}\" /OUTFILE:\"{1}\" /SYMPATH:\"{2}\" /EXEPATH:\"{3}\"", _dataCoverageFile, _codeCoverageXml, outTestDirectory, outTestDirectory);

            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = coverageExePath;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.Arguments = coverageExeArguments;
                this.LogBuildMessage("Running " + proc.StartInfo.FileName + " " + proc.StartInfo.Arguments);
                proc.Start();

                string errorStream = proc.StandardError.ReadToEnd();
                if (errorStream.Length > 0)
                {
                    if (errorStream.StartsWith("warning"))
                    {
                        this.LogBuildWarning(errorStream);
                        this.response.Set(this.ActivityContext, Library.CodeCoverage.Enums.Response.CodeCoverageConsoleWarning);
                    }
                    else
                    {
                        this.LogBuildError(errorStream);
                        this.response.Set(this.ActivityContext, Library.CodeCoverage.Enums.Response.CodeCoverageConsoleError);
                    }
                }

                string outputStream = proc.StandardOutput.ReadToEnd();
                if (outputStream.Length > 0)
                {
                    this.LogBuildMessage(outputStream);
                }

                proc.WaitForExit();
            }
        }

        private string GetPathTestRun()
        {
            var testRootPath = this.TestRootPath.Get(this.ActivityContext);

            if (Directory.Exists(testRootPath))
            {
                string[] testFiles = Directory.GetFiles(testRootPath, "*.trx");
                if (testFiles.Count() > 0)
                {
                    return Path.GetFileNameWithoutExtension(testFiles[0]);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
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
    }
}
