using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.Text.RegularExpressions;
using System.IO;
using TFSBuildExtensions.Helper;
using System.Xml.Serialization;
using TFSBuildExtensions.Library.Compression;

namespace TFSBuildExtensions.Compression
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class JsCssMinActivity : BaseCodeActivity
    {
        [Description("Fullpath Vueling.Minifier.ConsoleUI")]
        [RequiredArgument]
        public InArgument<string> FullPathVuelingMinifierConsoleUI { get; set; }

        [Description(" Path where file to compress is placed")]
        [RequiredArgument]
        public InArgument<string> Folder { get; set; }

        [Description("Compress js file")]
        public InArgument<bool> CompressjsFile { get; set; }

        [Description("Compress css file")]
        public InArgument<bool> CompresscssFile { get; set; }

        [Description("Excluded files containing this string")]
        [RequiredArgument]
        public InArgument<string[]> ExcludedFiles { get; set; }

        protected override void InternalExecute()
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

            pProcess.StartInfo.FileName = this.FullPathVuelingMinifierConsoleUI.Get(this.ActivityContext);
            pProcess.StartInfo.Arguments = String.Format("-directory {0}{1}{0} -minifycss {0}{2}{0} -minifyjs {0}{3}{0} -exceptions {0}{4}{0}", @"""", this.Folder.Get(this.ActivityContext), this.CompresscssFile.Get(this.ActivityContext), this.CompressjsFile.Get(this.ActivityContext), getExcludedFilesToString());

            LogBuildMessage(String.Format("Running command: {5} -directory {0}{1}{0} -minifycss {0}{2}{0} -minifyjs {0}{3}{0} -exceptions {0}{4}{0}", @"""", this.Folder.Get(this.ActivityContext), this.CompresscssFile.Get(this.ActivityContext), this.CompressjsFile.Get(this.ActivityContext), getExcludedFilesToString(), this.FullPathVuelingMinifierConsoleUI.Get(this.ActivityContext)));

            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.RedirectStandardError = true;
            pProcess.Start();

            string errorStream = pProcess.StandardError.ReadToEnd();
            if (errorStream.Length > 0)
            {
                LogBuildError(errorStream);
            }

            string strOutput = pProcess.StandardOutput.ReadToEnd();
            pProcess.WaitForExit();
            pProcess.Close();

            LogBuildMessage(strOutput);
        }

        internal string getExcludedFilesToString()
        {
            StringBuilder excludedFiles = new StringBuilder();

            foreach (var excl in this.ExcludedFiles.Get(this.ActivityContext))
            {
                excludedFiles.Append(excl);
                excludedFiles.Append(",");
            }

            return excludedFiles.ToString();
        }
    }
}
