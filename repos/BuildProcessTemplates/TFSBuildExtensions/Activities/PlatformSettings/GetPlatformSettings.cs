using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO;
using Vueling.Build.Contracts.ServiceLibrary;
using Vueling.Build.DB.Infrastructure.ADORepositories;
using Vueling.Build.DB.Infrastructure.Configuration;
using Vueling.Build.Impl.ServiceLibrary;

namespace TFSBuildExtensions.PlatformSettings
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public class GetPlatformSettings : BaseCodeActivity
    {
        private Vueling.Build.Contracts.ServiceLibrary.DTO.PlatformSettings platformSettings;
        private BuildPlatformSettingsService buildPlatformSettingsService;
        private BuildInfrastructureConfiguration buildInfrastructureConfiguration;
        private GetBuildDataRepository getBuildDataRepository;
        private GetServerDataRepository getServerDataRepository;

        [Description("Server where an application is deployed.")]
        [RequiredArgument]
        public InArgument<string> Server { get; set; }

        [Description("Website where an application is deployed.")]
        [RequiredArgument]
        public InArgument<string> Website { get; set; }

        [Description("Application Type (Application name)")]
        [RequiredArgument]
        public InArgument<string> ApplicationType { get; set; }

        [Description("Server where an application is deployed.")]
        public OutArgument<string> PlatformSettings { get; set; }

        [Description("Object platform settings")]
        public OutArgument<TFSBuildExtensions.DTO.PlatformSettings> PlatformSettingsObject { get; set; }

        protected override void InternalExecute()
        {
            ConcurrentDictionary<string, string> configurations = new ConcurrentDictionary<string, string>();

            if (buildPlatformSettingsService == null)
            {
                buildInfrastructureConfiguration = new BuildInfrastructureConfiguration();
                getBuildDataRepository = new GetBuildDataRepository(buildInfrastructureConfiguration);
                getServerDataRepository = new GetServerDataRepository(buildInfrastructureConfiguration);
                buildPlatformSettingsService = new BuildPlatformSettingsService(getBuildDataRepository, getServerDataRepository);
            }

            platformSettings = buildPlatformSettingsService.getPlatformSettings(this.Server.Get(this.ActivityContext), this.Website.Get(this.ActivityContext), this.ApplicationType.Get(this.ActivityContext));

            var mapper = MappingFromExternalDTOFactory.GetFor(EnumExternalDTOToLocalDTO.PlatformSettings);

            TFSBuildExtensions.DTO.PlatformSettings mappedPlatformSettings = mapper.Get<Vueling.Build.Contracts.ServiceLibrary.DTO.PlatformSettings, TFSBuildExtensions.DTO.PlatformSettings>(platformSettings);

            this.PlatformSettings.Set(this.ActivityContext, CommandlineStringTransformations.mapPlatformSettingsToCommandLineString(platformSettings));
            this.PlatformSettingsObject.Set(this.ActivityContext, mappedPlatformSettings);
        }
    }
}
