using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using System.IO;

namespace TFSBuildExtensions.EnvironmentManagement.Validation
{
    public class ConfigurationContentFileValidation : BaseCodeActivity
    {
        #region .:Public Properties

        [Description("Environment")]
        [RequiredArgument]
        public InArgument<string> Environment { get; set; }

        [Description("File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Description("Validation Result True:OK, False:KO")]
        [RequiredArgument]
        public OutArgument<bool> ValidationResult { get; set; }

        #endregion

        protected override void InternalExecute()
        {
            ValidationResult.Set(this.ActivityContext, true);

            loadContent(FilePath.Get(this.ActivityContext));
        }

        private void loadContent(string filePath)
        {
            if(File.Exists(filePath))
            {
                validateStringAccordingToEnvironment(File.ReadAllText(filePath));
            }
            else
            {
                LogBuildWarning("File " + filePath + " does not exists to validate its content according to environment.");
            }
        }

        private void validateStringAccordingToEnvironment(string contentFile)
        {
            switch (Environment.Get(this.ActivityContext))
            {
                case "INT":
                    ValidationResult.Set(this.ActivityContext, validateString(new string[] { "apps-bilbo.vueling.com", "skysales-bilbo.vueling.com", "batch.vueling.com", "private.vueling.com", "public.vueling.com", "https://vyr3xapi.navitaire.com" }, contentFile));
                    break;
                case "PRE":
                    ValidationResult.Set(this.ActivityContext, validateString(new string[] { "apps-aragorn.vueling.com", "skysales-aragorn.vueling.com", "batch.vueling.com", "private.vueling.com", "public.vueling.com", "https://vyr3xapi.navitaire.com" }, contentFile));
                    break;
                case "PRO":
                    ValidationResult.Set(this.ActivityContext, validateString(new string[] { "apps-aragorn.vueling.com", "skysales-aragorn.vueling.com", "apps-bilbo.vueling.com", "skysales-bilbo.vueling.com", "https://vytrainr3xapi.navitaire.com", "https://vytestr3xapi.navitaire.com", "apitest" }, contentFile));
                    break;
            }
        }

        private bool validateString(string[] findkey,string content)
        {
            return !findkey.Any(c => content.Contains(c));
        }
    }
}
