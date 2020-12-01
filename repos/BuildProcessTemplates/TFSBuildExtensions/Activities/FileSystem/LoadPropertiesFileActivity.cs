using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.IO;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using TFSBuildExtensions.Library.CodeMetrics.Report;
using System.Globalization;
using System.Diagnostics;
using TFSBuildExtensions.Library.CodeMetrics.Threshold;

namespace TFSBuildExtensions.FileSystem
{    
    [BuildActivity(HostEnvironmentOption.All)]
    public class LoadPropertiesFileActivity : BaseCodeActivity
    {
        /// <summary>
        /// Path to where the properties file is placed
        /// </summary>
        [Description(" Path to where the properties file is placed")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        /// <summary>
        /// Properties
        /// </summary>        
        [Description("Properties")]
        [RequiredArgument]
        public OutArgument<Dictionary<string, string>> Properties { get; set; }

        /// <summary>
        /// Executes the logic for this workflow activity
        /// </summary>
        protected override void InternalExecute()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            if (System.IO.File.Exists(this.FilePath.Get(this.ActivityContext)))
            {
                List<string> lines = System.IO.File.ReadLines(this.FilePath.Get(this.ActivityContext)).ToList();
                foreach (string line in lines)
                {
                    string[] values = line.Split('=');
                    if (values.Length == 2)
                    {
                        properties.Add(values[0], values[1]);
                    }
                }
            }
            else
            {
                string ipServer = Path.GetFileNameWithoutExtension(this.FilePath.Get(this.ActivityContext)).Substring(0, Path.GetFileNameWithoutExtension(this.FilePath.Get(this.ActivityContext)).Length - 4);
                this.LogBuildWarning(String.Format("File {0} does not exists, default values to {1} iis, sql and ftp properties files will be assigned.", this.FilePath.Get(this.ActivityContext), ipServer));

                properties.Add("server.iis.ip", ipServer);
                properties.Add("server.iis.static.ip", ipServer);
                properties.Add("server.iis.cms.front", ipServer);
                properties.Add("server.iis.deployuser", @".\tfsbuild");
                properties.Add("server.iis.deploypassword", "Bu1ld1ng");
                properties.Add("server.iis.backup.dir", @"\\${server.iis.ip}\c$\BackupWeb\");
                properties.Add("server.iis.remote.deploy.dir", @"\\${server.iis.ip}\c$\Repositorio_Web\");
                properties.Add("server.iis.local.deploy.dir", @"C:\Repositorio_Web\");
                properties.Add("server.iis.remote.deploy.static.dir", @"\\${server.iis.static.ip}\c$\Repositorio_Web\");
                properties.Add("server.iis.remote.deploy.config.dir", @"c$\Repositorio_Web\");
                properties.Add("server.iis.cms.remote.deploy.config.dir", @"c$\Repositorio_Web\");
                properties.Add("server.iis.website", "Default Web Site");
                properties.Add("server.sql.ip", ipServer);
                properties.Add("server.sql.port", "1433");
                properties.Add("server.sql.deployuser", "l.Hudson");
                properties.Add("server.sql.deploypassword", "Hudson_1804_L");
                properties.Add("server.sql.allowdrops", "True");
                properties.Add("server.ftp.ip", ipServer);
                properties.Add("server.ftp.user", "tfsbuild");
                properties.Add("server.ftp.password", "Bu1ld1ng");
            }
            this.Properties.Set(this.ActivityContext, properties);
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
