using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Impl.ServiceLibrary.Validation;

namespace TFSBuildExtensions.Validation
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class XmlTextValidation : BaseCodeActivity
    {
        [Description("XmlFilePath")]
        [RequiredArgument]
        public InArgument<string> XmlFilePath { get; set; }

        [Description("Result")]
        [RequiredArgument]
        public InOutArgument<int> Result { get; set; }

        protected override void InternalExecute()
        {
            XmlTextValidationService xmlTextValidationService = new XmlTextValidationService();

            xmlTextValidationService.Initialize(this.XmlFilePath.Get(this.ActivityContext));

            xmlTextValidationService.InternalExecute();

            this.Result.Set(this.ActivityContext, xmlTextValidationService.Result);

            this.InformationMessageList = xmlTextValidationService.InformationMessageList;
            this.WarningMessageList = xmlTextValidationService.WarningMessageList;
            this.ErrorMessageList = xmlTextValidationService.ErrorMessageList;
        }
    }
}