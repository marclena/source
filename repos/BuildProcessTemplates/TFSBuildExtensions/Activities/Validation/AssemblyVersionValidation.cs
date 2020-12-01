using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Activities;
using Microsoft.TeamFoundation.VersionControl.Client;
using TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using Vueling.Activities.Impl.ServiceLibrary.Validation;
using Vueling.Activities.Contracts.ServiceLibrary.Validation;

namespace TFSBuildExtensions.EnvironmentManagement.Validation
{
    public class AssemblyVersionValidation : BaseCodeActivity
    {
        #region .: Public properties :.

        [Description("List of projects to evaluate")]
        [RequiredArgument]
        public InArgument<List<string>> ProjectList { get; set; }

        [Description("Sources directory needed to get packages.config of each project")]
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [Description("Environment")]
        [RequiredArgument]
        public InArgument<string> Environment { get; set; }

        [Description("Assembly list which its version revision is not verified. Can be a list of files or file match patterns. Defaults to *")]
        public InArgument<IEnumerable<string>> AssemblyExceptions { get; set; }

        [Description("Which files that should be processed. Can be a list of files or file match patterns. Defaults to *")]
        public InArgument<IEnumerable<string>> AssembliesToProcess { get; set; }

        [Description("Assembly version are valid or not.")]
        public OutArgument<bool> AreVersionValid { get; set; }

        [Description("Full path lib assembly exceptions. This file contains XML configuration file.")]
        public InArgument<string> FullPathLibAssemblyExceptions { get; set; }

        [Description("Full path filtered libraries. This file contains json configuration file.")]
        [RequiredArgument]
        public InArgument<string> FullPathFilteredAssemblies { get; set; }

        #endregion

        protected override void InternalExecute()
        {
            IAssemblyVersionValidationService assemblyVersionValidationService = new AssemblyVersionValidationService();
            
            assemblyVersionValidationService.Initialize(this.ProjectList.Get(this.ActivityContext), 
                                                        this.SourcesDirectory.Get(this.ActivityContext), 
                                                        this.Environment.Get(this.ActivityContext), 
                                                        this.AssemblyExceptions.Get(this.ActivityContext), 
                                                        this.AssembliesToProcess.Get(this.ActivityContext), 
                                                        this.FullPathLibAssemblyExceptions.Get(this.ActivityContext),
                                                        this.FullPathFilteredAssemblies.Get(this.ActivityContext));

            assemblyVersionValidationService.InternalExecute();

            this.AreVersionValid.Set(this.ActivityContext, assemblyVersionValidationService.AreVersionValid);

            foreach (var message in assemblyVersionValidationService.LogErrorList)
            {
                LogBuildError(message);
            }

            foreach (var message in assemblyVersionValidationService.LogWarningList)
            {
                LogBuildWarning(message);
            }

            foreach (var message in assemblyVersionValidationService.LogInformationList)
            {
                LogBuildMessage(message);
            }
        }
    }
}
