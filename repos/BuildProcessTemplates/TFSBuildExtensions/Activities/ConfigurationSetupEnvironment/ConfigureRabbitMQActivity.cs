using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Xml.Linq;
using Vueling.Activities.Configuration;
using Vueling.Messaging.RabbitMqSecurity.Library.DomainServicesImplementations;
using Vueling.Messaging.RabbitMqSecurity.Library.Entities;

namespace TFSBuildExtensions.ConfigurationSetupEnvironment
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public class ConfigureRabbitMQActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> ConfigurationPath { get; set; }

        [RequiredArgument]
        public InArgument<string> Environment { get; set; }

        protected override void InternalExecute()
        {
            string fullPath = this.ConfigurationPath.Get(this.ActivityContext);

            var request = new SecurityProvisioningRequest()
            {
                Credentials = new DeployCredential { Name = Configuration.RabbitMQAdministratorsUser, Password = Configuration.RabbitMQAdministratorsPassword },
                ApplicationId = "Vueling.Activities.BuildProcess",
                GlobalConfigurationFile = XDocument.Load(fullPath)
            };

            var securityProvisioningService = InstanceFactory.CreateSecurityProvisioningService();
            securityProvisioningService.ProvisionApplications(request);
        }
    }
}
