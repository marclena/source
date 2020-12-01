using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Compression;
using Vueling.Activities.Impl.ServiceLibrary.Compression;

namespace TFSBuildExtensions.Compression
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class ZipActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> CompressedFilePath { get; set; }

        [RequiredArgument]
        public InArgument<List<string>> IncludeFiles { get; set; }

        public InArgument<List<string>> IgnoreFiles { get; set; }

        protected override void InternalExecute()
        {
            LogBuildMessage("Compressing files into ZIP to deploy...");
            IZipService zipService = new ZipService();

            zipService.Initialize(this.CompressedFilePath.Get(this.ActivityContext),
                this.IncludeFiles.Get(this.ActivityContext),
                this.IgnoreFiles.Get(this.ActivityContext));
            zipService.InternalExecute();

            this.InformationMessageList = zipService.InformationMessageList;
            this.WarningMessageList = zipService.WarningMessageList;
            this.ErrorMessageList = zipService.ErrorMessageList;
        }

    }
}
