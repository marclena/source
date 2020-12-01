using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.Workflow.Activities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO;
using Vueling.Build.Contracts.ServiceLibrary;
using Vueling.Build.Contracts.ServiceLibrary.DTO;
using Vueling.Build.DB.Infrastructure.ADORepositories;
using Vueling.Build.DB.Infrastructure.Configuration;
using Vueling.Build.Impl.ServiceLibrary;
using Vueling.Build.Impl.ServiceLibrary.Repositories;

namespace TFSBuildExtensions.PlatformSettings
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public class GetFunctionalComponentSettings : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> BuildName { get; set; }

        [RequiredArgument]
        public OutArgument<StringList> ApplicationToServerMatching { get; set; }

        IBuildApplicationOnPremiseSettingsService buildApplicationOnPremiseSettingsService;
        IGetApplicationDataRepository getApplicationDataRepository;
        IBuildInfrastructureConfiguration buildInfrastructureConfiguration;

        protected override void InternalExecute()
        {
            string buildName = this.BuildName.Get(this.ActivityContext);

            InitActivity();

            List<ApplicationOnPremiseSetting> applicationOnPremiseSettings = buildApplicationOnPremiseSettingsService.ApplicationOnPremiseSettings(buildName);

            var mapper = MappingFromExternalDTOFactory.GetFor(EnumExternalDTOToLocalDTO.ApplicationToServerMatching);

            var applicationsMapped = mapper.Get<List<Vueling.Build.Contracts.ServiceLibrary.DTO.ApplicationOnPremiseSetting>, StringList>(applicationOnPremiseSettings);

            this.ApplicationToServerMatching.Set(this.ActivityContext, applicationsMapped);
        }

        internal void InitActivity()
        {
            buildInfrastructureConfiguration = new BuildInfrastructureConfiguration();
            getApplicationDataRepository = new GetApplicationDataRepository(buildInfrastructureConfiguration);
            buildApplicationOnPremiseSettingsService = new BuildApplicationOnPremiseSettingsService(getApplicationDataRepository);
        }
    }
}
