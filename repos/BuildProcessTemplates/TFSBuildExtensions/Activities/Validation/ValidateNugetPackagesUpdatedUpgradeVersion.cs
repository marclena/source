using System;
using Microsoft.TeamFoundation.Build.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Activities;
using System.IO;
using Vueling.NugetManager.ServiceLibrary.Helper;

namespace TFSBuildExtensions.Validation
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class ValidateNugetPackagesUpdatedUpgradeVersion : BaseCodeActivity
    {
        [Description("CheckedInFiles")]
        [RequiredArgument]
        public InArgument<IList<string>> CheckedInFilesLocalItem { get; set; }

        [Description("ProjectNames")]
        [RequiredArgument]
        public InArgument<List<string>> ProjectNames { get; set; }

        [Description("SourcesDirectory")]
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [Description("SourcesDirectory")]
        [RequiredArgument]
        public InOutArgument<bool> Result { get; set; }

        protected override void InternalExecute()
        {
            this.Result.Set(this.ActivityContext, true);

            List<string> projectsGenerateNugetPackage = new List<string>();

            foreach (var projectName in this.ProjectNames.Get(this.ActivityContext))
            {
                if (ProjectGenerateNugetPackage(projectName))
                {
                    projectsGenerateNugetPackage.Add(projectName);
                }
            }

            foreach (var projectName in projectsGenerateNugetPackage)
            {
                if (this.CheckedInFilesLocalItem.Get(this.ActivityContext).Contains("\\" + projectName + "\\"))
                {
                    if (!this.CheckedInFilesLocalItem.Get(this.ActivityContext).Contains(this.SourcesDirectory.Get(this.ActivityContext) + "\\" + projectName + "\\Properties\\AssemblyInfo.cs"))
                    {
                        ErrorMessageList.Add("Nuget package version " + projectName + " must be upgraded.");
                        this.Result.Set(this.ActivityContext, false);
                    }
                }
            }
        }

        private bool ProjectGenerateNugetPackage(string projectName)
        {
            foreach (string nugetPackageProjectType in Vueling.Activities.Configuration.Configuration.NugetPackagesProjectTypes.Split(','))
            {
                if (projectName.ToLower().EndsWith(nugetPackageProjectType.ToLower()))
                {
                    string pathAssemblyInfo = this.SourcesDirectory.Get(this.ActivityContext) + "\\" + projectName + "\\Properties\\AssemblyInfo.cs";
                    if (File.Exists(pathAssemblyInfo) && ValidateAssemblyInfoFile.IsNugetGenerationEnabled(projectName, pathAssemblyInfo))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}