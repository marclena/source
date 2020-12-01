using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TFSBuildExtensions.NPM
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public class RunNPMCommandsActivity : BaseCodeActivity 
    {
        [RequiredArgument]
        public InArgument<string[]> Commands { get; set; }

        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        protected override void InternalExecute()
        {
            string[] Commands = this.Commands.Get(this.ActivityContext);
            string sourcesDirectory = this.SourcesDirectory.Get(this.ActivityContext);

            this.InformationMessageList = new List<string>();
            this.WarningMessageList = new List<string>();
            this.ErrorMessageList = new List<string>();

            foreach (var npmCommand in Commands)
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) + "\\nodejs\\npm.cmd",
                    Arguments = npmCommand,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = sourcesDirectory,
                    CreateNoWindow = false
                };

                if(!File.Exists(processStartInfo.FileName))
                {
                    throw new FileNotFoundException("File " + processStartInfo.FileName + " not found");
                }

                RunCommandLineProcess(processStartInfo);
            }
        }

        private void RunCommandLineProcess(ProcessStartInfo processStartInfo)
        {
            this.InformationMessageList.Add("Running command " + processStartInfo.FileName + " " + processStartInfo.Arguments);

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();


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
                            if (e.Data.Contains("npm ERR"))
                            {
                                this.ErrorMessageList.Add(e.Data);
                            }
                            else
                            {
                                this.InformationMessageList.Add(e.Data);
                            }
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
                            this.ErrorMessageList.Add(e.Data);
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

            watch.Stop();

            var elapsed = watch.Elapsed;

            this.InformationMessageList.Add("Command run for " + elapsed);
        }
    }
}
