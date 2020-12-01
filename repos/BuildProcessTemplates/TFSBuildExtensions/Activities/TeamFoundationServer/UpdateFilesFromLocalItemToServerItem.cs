using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.TeamFoundationServer;
using Vueling.Activities.Impl.ServiceLibrary.TeamFoundationServer;

namespace TFSBuildExtensions.TeamFoundationServer
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class UpdateFilesFromLocalItemToServerItem : BaseCodeActivity
    {
        [Description("")]
        [RequiredArgument]
        public InArgument<List<string>> SourceFiles { get; set; }

        [Description("")]
        [RequiredArgument]
        public InArgument<Workspace> Workspace { get; set; }

        [Description("")]
        [RequiredArgument]
        public InArgument<List<string>> TargetServerItems { get; set; }

        protected override void InternalExecute()
        {
            IUpdateFilesFromLocalItemToServerItemService updateFilesFromLocalItemToServerItemService = new UpdateFilesFromLocalItemToServerItemService(this.SourceFiles.Get(this.ActivityContext), this.Workspace.Get(this.ActivityContext), this.TargetServerItems.Get(this.ActivityContext));

            updateFilesFromLocalItemToServerItemService.InternalExecute();

            this.InformationMessageList = updateFilesFromLocalItemToServerItemService.InformationMessageList;
            this.WarningMessageList = updateFilesFromLocalItemToServerItemService.WarningMessageList;
            this.ErrorMessageList = updateFilesFromLocalItemToServerItemService.ErrorMessageList;
        }
    }
}
