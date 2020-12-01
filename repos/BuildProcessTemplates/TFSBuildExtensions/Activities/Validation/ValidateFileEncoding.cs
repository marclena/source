using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.ConfigurationSetupEnvironment;
using Vueling.Activities.Contracts.ServiceLibrary.Validation;
using Vueling.Activities.Impl.ServiceLibrary.Validation;

namespace TFSBuildExtensions.Validation
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class ValidateFileEncoding : BaseCodeActivity
    {
        [Description("FilePath")]
        [RequiredArgument]
        public InArgument<List<string>> FilesPath { get; set; }

        [Description("Result")]
        [RequiredArgument]
        public InOutArgument<int> Result { get; set; }

        [Description("VersionControlServer")]
        [RequiredArgument]
        public InArgument<VersionControlServer> VersionControlServer { get; set; }

        [Description("ExcludedExtensions")]
        public InArgument<List<string>> ExcludedExtensions { get; set; }


        protected override void InternalExecute()
        {
            List<string> files = FilterFilesWithExcludedExtensions();

            IValidateFileEncodingService validateFileEncodingService = new ValidateFileEncodingService();

            validateFileEncodingService.Initialize(files);

            validateFileEncodingService.InternalExecute();

            this.Result.Set(this.ActivityContext, validateFileEncodingService.Result);

            this.InformationMessageList = validateFileEncodingService.InformationMessageList;
            this.WarningMessageList = validateFileEncodingService.WarningMessageList;
            this.ErrorMessageList = validateFileEncodingService.ErrorMessageList;
        }

        private List<string> FilterFilesWithExcludedExtensions()
        {
            List<string> filteredFiles = new List<string>();

            foreach (var file in this.FilesPath.Get(this.ActivityContext))
            {
                string extension = Path.GetExtension(file);
                if (!ConfigurationWorkflow.ValidateFileEncodingExcludedExtensions.Contains(extension))
                {
                    filteredFiles.Add(file);
                }
            }

            return filteredFiles;
        }
    }
}