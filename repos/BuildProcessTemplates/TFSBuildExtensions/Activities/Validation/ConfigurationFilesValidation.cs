using System;
using System.Activities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.TeamFoundation.Build.Client;

namespace TFSBuildExtensions.Validation
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class ConfigurationFilesValidation : BaseCodeActivity
    {
        public InArgument<List<string>> FilesToProcess { get; set; }

        public InArgument<string> Environment { get; set; }

        public InArgument<bool> ForceFullDeploy { get; set; }

        public OutArgument<List<string>> Errors { get; set; }

        public OutArgument<List<string>> CheckedInFilesToDeploy { get; set; }

        public OutArgument<List<string>> IOMs { get; set; }

        public OutArgument<bool> UploadedFilesNeedRunAutoSetupEnvironment { get; set; }

        public OutArgument<bool> ContainsGlobalOrUsers { get; set; }

        public OutArgument<bool> ConfigureRabbitMQ { get; set; }

        protected override void InternalExecute()
        {
            ConcurrentBag<string> concurrentErrorList = new ConcurrentBag<string>();
            ConcurrentBag<string> checkedInFilesToDeploy = new ConcurrentBag<string>();
            ConcurrentBag<string> ioms = new ConcurrentBag<string>();

            if (this.ForceFullDeploy.Get(this.ActivityContext))
            {
                this.UploadedFilesNeedRunAutoSetupEnvironment.Set(this.ActivityContext, true);
                this.ContainsGlobalOrUsers.Set(this.ActivityContext, true);
            }

            Parallel.ForEach(this.FilesToProcess.Get(this.ActivityContext), configurationFile =>
            {
                if(File.Exists(configurationFile) && configurationFile.Contains("Vueling.Configuration.Config"))
                {
                    if (!IsXmlValid(configurationFile)) 
                    {
                        concurrentErrorList.Add(String.Format("File {0} has incorrect xml syntax", configurationFile));
                    }
                    
                    if (!ValidateContentFile(configurationFile))
                    {
                        concurrentErrorList.Add(String.Format("File {0} has incorrect content according to environment configured in the build definition.", configurationFile));
                    }

                    if (configurationFile.Contains("iom.xml"))
                    {
                        this.UploadedFilesNeedRunAutoSetupEnvironment.Set(this.ActivityContext, true);
                        ioms.Add(configurationFile);
                    }

                    if (configurationFile.Contains(@"\Vueling.Configuration.Config\global"))
                    {
                        this.ContainsGlobalOrUsers.Set(this.ActivityContext, true);
                        this.UploadedFilesNeedRunAutoSetupEnvironment.Set(this.ActivityContext, true);
                        checkedInFilesToDeploy.Add(Path.Combine(Path.GetDirectoryName(configurationFile) + @"\..\", "global.config"));
                    }

                    if (configurationFile.EndsWith("users.config") || configurationFile.Contains("global.config"))
                    {
                        this.ContainsGlobalOrUsers.Set(this.ActivityContext, true);
                        this.UploadedFilesNeedRunAutoSetupEnvironment.Set(this.ActivityContext, true);
                        this.ContainsGlobalOrUsers.Set(this.ActivityContext, true);
                    }

                    if (configurationFile.EndsWith("rabbitmq.xml"))
                    {
                        this.ConfigureRabbitMQ.Set(this.ActivityContext, true);
                    }

                    checkedInFilesToDeploy.Add(configurationFile);
                }
            });

            this.Errors.Set(this.ActivityContext, concurrentErrorList.ToList());
            this.CheckedInFilesToDeploy.Set(this.ActivityContext, checkedInFilesToDeploy.ToList());
            this.IOMs.Set(this.ActivityContext, ioms.ToList());
        }

        internal bool IsXmlValid(string fullPathFile)
        {
            try
            {
                if (Path.GetExtension(fullPathFile).EndsWith("xml") || Path.GetExtension(fullPathFile).EndsWith("config")
                    || Path.GetExtension(fullPathFile).EndsWith("naml") || Path.GetExtension(fullPathFile).EndsWith("xslt"))
                {
                    XDocument.Load(fullPathFile);
                }
            }
            catch (XmlException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        internal bool ValidateContentFile(string filePath)
        {
            bool result = true;

            string contentFile = File.ReadAllText(filePath);

            switch (Environment.Get(this.ActivityContext))
            {
                case "INT":
                    result = validateString(new string[] { "apps-bilbo.vueling.com", "skysales-bilbo.vueling.com", "batch.vueling.com", "private.vueling.com", "public.vueling.com", "https://vyr3xapi.navitaire.com" }, contentFile);
                    break;
                case "PRE":
                    result = validateString(new string[] { "apps-aragorn.vueling.com", "skysales-aragorn.vueling.com", "batch.vueling.com", "private.vueling.com", "public.vueling.com", "https://vyr3xapi.navitaire.com" }, contentFile);
                    break;
                case "PRO":
                    result = validateString(new string[] { "apps-aragorn.vueling.com", "skysales-aragorn.vueling.com", "apps-bilbo.vueling.com", "skysales-bilbo.vueling.com", "https://vytrainr3xapi.navitaire.com", "https://vytestr3xapi.navitaire.com", "apitest" }, contentFile);
                    break;
            }

            return result;
        }

        private bool validateString(string[] findkey, string content)
        {
            return !findkey.Any(c => content.Contains(c));
        }

    }
}
