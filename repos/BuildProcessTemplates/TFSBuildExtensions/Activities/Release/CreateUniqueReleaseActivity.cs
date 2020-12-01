using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Release;
using Vueling.Activities.Impl.ServiceLibrary.Release;

namespace TFSBuildExtensions.Release
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class CreateUniqueReleaseActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        [RequiredArgument]
        public InArgument<string> Source { get; set; }
        
        [RequiredArgument]
        public InArgument<Workspace> Workspace { get; set; }

        protected override void InternalExecute()
        {
            ICreateUniqueReleaseItemService createUniqueReleaseItemService = new CreateUniqueReleaseItemService();

            createUniqueReleaseItemService.Initialize(this.BinariesDirectory.Get(this.ActivityContext), this.Source.Get(this.ActivityContext), this.Workspace.Get(this.ActivityContext));

            createUniqueReleaseItemService.InternalExecute();
        }
    }
}