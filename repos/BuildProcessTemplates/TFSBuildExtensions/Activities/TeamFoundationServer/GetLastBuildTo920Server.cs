using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;

namespace TFSBuildExtensions.TeamFoundationServer
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetLastBuildTo920Server : BaseCodeActivity
    {

        [Description("Last snapshot deployed to Navitaire 920 server.")]
        public OutArgument<string> CurrentSnapshot920Server { get; set; }

        private TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection tfsInstance;
        private IBuildServer buildServer;

        protected override void InternalExecute()
        {
            tfsInstance = new TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection();

            buildServer = (IBuildServer)tfsInstance.Server.GetService(typeof(IBuildServer));

            IBuildDefinition skysales920 = buildServer.GetBuildDefinition(new Uri("vstfs:///Build/Definition/1013"));
            IBuildDefinition skysales920Branch = buildServer.GetBuildDefinition(new Uri("vstfs:///Build/Definition/1101"));

            if (GetIdFromUriString(skysales920.LastGoodBuildUri.ToString()) > GetIdFromUriString(skysales920Branch.LastGoodBuildUri.ToString()))
            {
                IBuildDetail lastbuild = buildServer.GetBuild(skysales920.LastGoodBuildUri);

                CurrentSnapshot920Server.Set(this.ActivityContext, lastbuild.BuildNumber);
            }
            else
            {
                IBuildDetail lastbuild = buildServer.GetBuild(skysales920Branch.LastGoodBuildUri);

                CurrentSnapshot920Server.Set(this.ActivityContext, lastbuild.BuildNumber);
            }

        }

        private int GetIdFromUriString(string uri)
        {
            int lastIndexOfSlash = uri.LastIndexOf("/");

            return int.Parse(uri.Substring(lastIndexOfSlash + 1, uri.Length - 1 - lastIndexOfSlash));
        }
    }
}
