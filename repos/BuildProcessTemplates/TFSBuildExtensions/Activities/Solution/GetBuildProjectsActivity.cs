using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.Activities;
using System.ComponentModel;
using Microsoft.TeamFoundation.Build.Workflow.Activities;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using System.IO;
using System.Xml.Linq;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Text.RegularExpressions;
using Vueling.Activities.Impl.ServiceLibrary.Solution;

namespace TFSBuildExtensions.Solution
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetBuildProjectsActivity : BaseCodeActivity
    {       
        /// <summary>
        /// ProjectsToBuild
        /// </summary>        
        [Description("ProjectsToBuild")]
        [RequiredArgument]
        public InArgument<StringList> ProjectsToBuild { get; set; }

        /// <summary>
        /// Workspace
        /// </summary>        
        [Description("Workspace")]
        [RequiredArgument]
        public InArgument<Workspace> Workspace { get; set; }

        /// <summary>
        /// WorkspaProjectsce
        /// </summary>        
        [Description("Projects")]
        [RequiredArgument]
        public InOutArgument<string[]> Projects { get; set; }

        [Description("Visual Studio Version")]
        public OutArgument<string> VersionVisualStudio { get; set; }


        /// <summary>
        /// Executes the logic for this workflow activity
        /// </summary>
        protected override void InternalExecute()
        {
            var iGetBuildProjectsService = new GetBuildProjectsService();

            iGetBuildProjectsService.Initialize(this.Workspace.Get(this.ActivityContext), this.ProjectsToBuild.Get(this.ActivityContext), null);

            iGetBuildProjectsService.InternalExecute();

            this.VersionVisualStudio.Set(this.ActivityContext, iGetBuildProjectsService.VersionVisualStudio);
            this.Projects.Set(this.ActivityContext, iGetBuildProjectsService.Projects);

            foreach (var message in iGetBuildProjectsService.Messages)
            {
                if (message.StartsWith("ERROR"))
                {
                    LogBuildError(message);
                }
                else
                {
                    LogBuildMessage(message);
                }
            }
        }
    }
}
