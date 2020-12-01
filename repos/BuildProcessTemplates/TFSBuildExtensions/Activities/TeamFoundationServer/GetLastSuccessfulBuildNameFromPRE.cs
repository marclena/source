using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;
using Vueling.Activities.Configuration;

namespace TFSBuildExtensions.TeamFoundationServer
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetLastSuccessfulBuildNameFromPRE : BaseCodeActivity
    {
        private TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection tfsInstance;
        private IBuildServer buildServer;

        [Description("Build definition Name")]
        public InOutArgument<string> BuildDefinitionName { get; set; }

        [Description("Get last successful build name")]
        public OutArgument<string> LastSuccessfulBuildName { get; set; }

        protected override void InternalExecute()
        {
            InitializeTFSInstance();

            string buildDefinitionNamePRE = null;

            if (String.IsNullOrEmpty(this.BuildDefinitionName.Get(this.ActivityContext)))
            {
                buildDefinitionNamePRE = BuildDetail.BuildDefinition.Name.Replace(".PRO.", ".PRE.");
                this.BuildDefinitionName.Set(this.ActivityContext, buildDefinitionNamePRE);
            }
            else
            {
                buildDefinitionNamePRE = this.BuildDefinitionName.Get(this.ActivityContext);
            }

            var buildDefinition = BuildDetail.BuildDefinition;

            try
            {
                buildDefinition = buildServer.GetBuildDefinition(Configuration.teamProject, buildDefinitionNamePRE);
            }

            catch (BuildDefinitionNotFoundException)
            {
                LogBuildWarning("Build definition buildDefinitionPRE does not exists.");
            }

            if (buildDefinition.LastGoodBuildUri == null)
            {
                LogBuildMessage(String.Format("Last successful build from {0} doesn't exists.,", buildDefinitionNamePRE));

                LastSuccessfulBuildName.Set(this.ActivityContext, String.Empty);
            }
            else
            {
                var lastBuild = buildServer.GetBuild(buildDefinition.LastGoodBuildUri);

                LogBuildMessage(String.Format("Last successful build from {0} is {1},", buildDefinitionNamePRE, lastBuild.BuildNumber));

                LastSuccessfulBuildName.Set(this.ActivityContext, lastBuild.BuildNumber);
            }
        }

        private void InitializeTFSInstance()
        {
            tfsInstance = new TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection();

            buildServer = (IBuildServer)tfsInstance.Server.GetService(typeof(IBuildServer));
        }
    }
}
