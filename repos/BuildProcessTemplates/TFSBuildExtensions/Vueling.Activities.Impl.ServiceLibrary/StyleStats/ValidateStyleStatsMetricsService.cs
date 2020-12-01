using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.StyleStats;
using Vueling.Activities.Contracts.ServiceLibrary;
using Vueling.Activities.Contracts.ServiceLibrary.StyleStats;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.StyleStats
{
    [RegisterServiceAttribute]
    public class ValidateStyleStatsMetricsService : BaseActivityService, IValidateStyleStatsMetrics
    {
        private string sourcesDirectory;
        private string binariesDirectory;
        private string fullPathJsonResults;
        private string fullPathJsonStyleStatsThreshold;
        private string searchPattern;
        private string[] excludedFilesPattern;
        private StyleStatsResults threshold;

        public void Initialize(string _sourcesDirectory, string _binariesDirectory, string _fullPathJsonResults, string _fullPathJsonResultsThreshold, string _searchPattern = "*css", string[] _excludedFilesPattern = null)
        {
            sourcesDirectory = _sourcesDirectory;
            binariesDirectory = _binariesDirectory;
            fullPathJsonResults = _fullPathJsonResults;
            fullPathJsonStyleStatsThreshold = _fullPathJsonResultsThreshold;
            searchPattern = _searchPattern;
            excludedFilesPattern = _excludedFilesPattern;
        }

        public override void InternalExecute()
        {
            LoadJsonFiles();

            runRecursiveStyleStats();

            ValidateStyleStatsResults();

            if (this.Result > 0)
            {
                this.ErrorMessageList.Add("Style Stats validation Fail.");
            }
        }

        private void LoadJsonFiles()
        {
            try
            {
                if (threshold == null)
                {
                    //Loads specific threshold configuration file for this application
                    if (File.Exists(Path.Combine(sourcesDirectory, "stylestats-threshold.json")))
                    {
                        fullPathJsonStyleStatsThreshold = Path.Combine(sourcesDirectory, "stylestats-threshold.json");
                    }

                    using (StreamReader read = new StreamReader(fullPathJsonStyleStatsThreshold))
                    {
                        string json = read.ReadToEnd();
                        threshold = JsonConvert.DeserializeObject<StyleStatsResults>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessageList.Add("Error parsing results, please review css parsing.");
                this.ErrorMessageList.Add(ex.ToString());

                this.Result = 1;
            }
        }

        private void runRecursiveStyleStats()
        {
            string[] files = Directory.GetFiles(sourcesDirectory, searchPattern, SearchOption.AllDirectories);

            try
            {
                Directory.Delete(binariesDirectory + @"\stylestatsresults", true);
            }
            catch (DirectoryNotFoundException)
            {
                //If folder doesn't exist continue
            }

            Directory.CreateDirectory(binariesDirectory + @"\stylestatsresults");

            foreach (var file in files)
            {
                if (!ExcludedFile(file))
                {
                    FileInfo fileInfo = new FileInfo(file);

                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = sourcesDirectory + @"\node_modules\.bin\stylestats.cmd",
                        Arguments = file + " -c stylestatsconfig.json -f json > " + binariesDirectory + @"\stylestatsresults\stylestats-results-" + Path.GetFileNameWithoutExtension(file) + ".json",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WorkingDirectory = sourcesDirectory,
                        CreateNoWindow = false
                    };

                    RunCommandLineProcess(processStartInfo);
                }
            }
        }

        private bool ExcludedFile(string filename)
        {
            if (excludedFilesPattern != null)
            {
                foreach (var pattern in excludedFilesPattern)
                {
                    if (filename.Contains(pattern))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void RunCommandLineProcess(ProcessStartInfo processStartInfo)
        {
            using (Process process = Process.Start(processStartInfo))
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
                            this.InformationMessageList.Add(e.Data);
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
                            this.InformationMessageList.Add(e.Data);
                        }
                    };
                    process.BeginErrorReadLine();
                    process.StandardInput.Close();
                    process.WaitForExit();

                    mreOut.WaitOne();
                    mreErr.WaitOne();

                    if (process.ExitCode != 0)
                    {
                        this.ErrorMessageList.Add(process.ExitCode.ToString(CultureInfo.CurrentCulture));
                    }
                }
            }
        }

        internal void ValidateStyleStatsResults()
        {
            string[] files = Directory.GetFiles(binariesDirectory + @"\stylestatsresults", "*json");

            foreach (var file in files)
            {
                using (StreamReader read = new StreamReader(file))
                {
                    string json = read.ReadToEnd();
                    StyleStatsResults stylestatsResults = null;

                    try
                    {
                        stylestatsResults = JsonConvert.DeserializeObject<StyleStatsResults>(json);
                    }
                    catch (JsonSerializationException)
                    {
                        this.ErrorMessageList.Add("Error parsing file " + file);
                        this.Result = 1;
                        break;
                    }

                    if (stylestatsResults == null)
                    {
                        this.WarningMessageList.Add("[WARNING] StyleStatsResults " + file + " does not exist, or cannot be deserialized.");
                    }
                    else
                    {
                        foreach (var property in threshold.GetType().GetProperties())
                        {
                            bool validThreshold;

                            switch (property.Name)
                            {
                                case "rules":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "selectors":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "simplicity":
                                    validThreshold = CompareDecimalTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "averageOfIdentifier":
                                    validThreshold = CompareDecimalTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "mostIdentifier":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "averageOfCohesion":
                                    validThreshold = CompareDecimalTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "lowestCohesion":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "totalUniqueFontSizes":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "totalUniqueFontFamilies":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "idSelectors":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "universalSelectors":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "unqualifiedAttributeSelectors":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "javascriptSpecificSelectors":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "importantKeywords":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "floatProperties":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                case "mediaQueries":
                                    validThreshold = CompareIntTypeThresholdBelowValue(property, stylestatsResults);
                                    break;
                                default:
                                    validThreshold = true;
                                    break;
                            }

                            if (!validThreshold)
                            {
                                ErrorMessageList.Add("Error! Property " + property.Name + " has incorrect value in file " + file);
                            }
                        }
                    }

                }
            }
        }

        private bool CompareIntTypeThresholdEqualsValue(System.Reflection.PropertyInfo property, StyleStatsResults stylestatsResults)
        {
            if (int.Parse(property.GetValue(threshold).ToString()) > 0 && int.Parse(property.GetValue(threshold).ToString()) != int.Parse(property.GetValue(stylestatsResults).ToString()))
            {
                this.Result = 1;

                ErrorMessageList.Add(String.Format("{0} threshold is {1} and  current {0} are {2}", property.Name, property.GetValue(threshold).ToString(), property.GetValue(stylestatsResults).ToString()));

                return false;
            }

            return true;
        }

        private bool CompareIntTypeThresholdBelowValue(System.Reflection.PropertyInfo property, StyleStatsResults stylestatsResults)
        {
            if (int.Parse(property.GetValue(threshold).ToString()) > 0 && int.Parse(property.GetValue(threshold).ToString()) < int.Parse(property.GetValue(stylestatsResults).ToString()))
            {
                this.Result = 1;

                ErrorMessageList.Add(String.Format("{0} threshold is {1} and  current {0} are {2}", property.Name, property.GetValue(threshold).ToString(), property.GetValue(stylestatsResults).ToString()));

                return false;
            }

            return true;
        }

        private bool CompareDecimalTypeThresholdBelowValue(System.Reflection.PropertyInfo property, StyleStatsResults stylestatsResults)
        {
            if (decimal.Parse(property.GetValue(threshold).ToString()) > 0 && decimal.Parse(property.GetValue(threshold).ToString()) < decimal.Parse(property.GetValue(stylestatsResults).ToString()))
            {
                this.Result = 1;

                ErrorMessageList.Add(String.Format("{0} threshold is {1} and  current {0} are {2}", property.Name, property.GetValue(threshold).ToString(), property.GetValue(stylestatsResults).ToString()));

                return false;
            }

            return true;
        }
    }
}