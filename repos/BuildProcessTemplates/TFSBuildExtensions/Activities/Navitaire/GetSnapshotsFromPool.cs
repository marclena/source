using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;
using Vueling.Activities.Configuration;

namespace TFSBuildExtensions.Navitaire
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetSnapshotsFromPool : BaseCodeActivity
    {
        [Description("Full path Vueling.Navitaire.Web.Deploy.Manager.ConsoleUI.exe application")]
        [RequiredArgument]
        public InArgument<string> FullPathVuelingNavitaireWebDeployManagerConsoleUI { get; set; }

        [Description("Pool name where activity finds previous snapshot.")]
        [RequiredArgument]
        public InArgument<string> Pool { get; set; }

        [Description("Active snapshot")]
        public OutArgument<string> ActiveSnapshot { get; set; }

        [Description("Previous snapshot")]
        public OutArgument<string> PreviousSnapshot { get; set; }

        protected override void InternalExecute()
        {
            string activeSnapshot = GetSnapshot("Active");
            this.ActiveSnapshot.Set(this.ActivityContext, activeSnapshot);

            string previousSnapshot = GetSnapshot("Previous");
            this.PreviousSnapshot.Set(this.ActivityContext, previousSnapshot);
        }

        private string GetSnapshot(string snapshot)
        {
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

            pProcess.StartInfo.FileName = this.FullPathVuelingNavitaireWebDeployManagerConsoleUI.Get(this.ActivityContext);
            pProcess.StartInfo.Arguments = String.Format("-server {0}{1}{0} -port {0}{2}{0} -pool {0}{3}{0} -snapshot {0}{4}{0}", @"""", Configuration.NavitaireWebDeployManagerServer, Configuration.NavitaireWebDeployManagerServerPort, this.Pool.Get(this.ActivityContext), snapshot);

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

            strOutput = strOutput.Substring(0, strOutput.Length - (strOutput.Length - strOutput.IndexOf(".wss")));
            return strOutput;
        }
    }
}
