using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.StaticContentHelper;
using Vueling.Activities.Impl.ServiceLibrary.StaticContentHelper;

namespace TFSBuildExtensions.StaticContentHelper
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class StaticContentHelperActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [RequiredArgument]
        public InArgument<string> ProjectName { get; set; }

        [RequiredArgument]
        public InArgument<Workspace> Workspace { get; set; }

        protected override void InternalExecute()
        {
            string source = this.SourcesDirectory.Get(this.ActivityContext) + "\\" + this.ProjectName.Get(this.ActivityContext) + ".WebUI";
            string target = this.SourcesDirectory.Get(this.ActivityContext) + "\\" + this.ProjectName.Get(this.ActivityContext) + ".Helpers.Impl.ServiceLibrary";
            string namespacename = this.ProjectName.Get(this.ActivityContext) + ".Helpers.Impl.ServiceLibrary";

            //checkout
            this.Workspace.Get(this.ActivityContext).PendEdit(target, RecursionType.Full);

            IStaticContentHelperGenerator staticContentHelperGeneratorService = new StaticContentHelperGeneratorService();

            staticContentHelperGeneratorService.Initialize(source, target, namespacename);

            staticContentHelperGeneratorService.InternalExecute();

            this.InformationMessageList = staticContentHelperGeneratorService.InformationMessageList;
            this.WarningMessageList = staticContentHelperGeneratorService.WarningMessageList;
            this.ErrorMessageList = staticContentHelperGeneratorService.ErrorMessageList;

            int numPendingChanges = this.Workspace.Get(this.ActivityContext).PendAdd(target + "\\*.cs", true);
            int numPendingEdit = this.Workspace.Get(this.ActivityContext).PendEdit(target + "\\*.cs", RecursionType.Full);

            this.InformationMessageList.Add("number of files to add with pendadd: " + numPendingChanges);

            if (numPendingChanges > 0 || numPendingEdit >0)
            {
                PendingChange[] pendingChanges = this.Workspace.Get(this.ActivityContext).GetPendingChanges(target, RecursionType.Full);

                foreach (var pc in pendingChanges)
                {
                    this.InformationMessageList.Add("Pending change: " + pc.LocalItem);
                }

                UpgradeAssemblyVersion(target);

                //rerun getpending changes to add assemblyinfo update
                pendingChanges = this.Workspace.Get(this.ActivityContext).GetPendingChanges(target, RecursionType.Full);

                CheckInFiles(pendingChanges);

                this.InformationMessageList.Add("files checkedin successfully");
            }
        }

        private void UpgradeAssemblyVersion(string target)
        {
            Regex assemblyInfoVersionExpression = new Regex("\\[assembly: AssemblyVersion\\(\"(?<version>((\\d)+\\.)+(\\d)+)\"\\)\\]");
            string version = null;
            string assemblyInfoText;

            string assemblyInfoPath = Path.Combine(target, "Properties\\AssemblyInfo.cs");

            using (StreamReader reader = new StreamReader(assemblyInfoPath))
            {
                assemblyInfoText = reader.ReadToEnd();

                if (assemblyInfoVersionExpression.IsMatch(assemblyInfoText))
                {
                    version = assemblyInfoVersionExpression.Match(assemblyInfoText).Groups[assemblyInfoVersionExpression.GroupNumberFromName("version")].Value;
                }

                string[] versionNumbers = version.Split(new char[] { '.' });

                int v = (int.Parse(versionNumbers[3].ToString())) + 1;

                string newVersion = versionNumbers[0] + "." + versionNumbers[1] + "." + versionNumbers[2] + "." + v.ToString();

                assemblyInfoText = assemblyInfoText.Replace(version, newVersion);
            }

            using (StreamWriter writer = new StreamWriter(assemblyInfoPath))
            {
                writer.Write(assemblyInfoText);
            }
        }

        private void CheckInFiles(PendingChange[] pendingChanges)
        {
            WorkspaceCheckInParameters parameters = new WorkspaceCheckInParameters(pendingChanges, "Automatic updated static helper files.")
            {
                OverrideGatedCheckIn = true,
            };

            this.Workspace.Get(this.ActivityContext).CheckIn(parameters);
        }
    }
}