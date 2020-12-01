using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO;
using Vueling.Build.DB.Infrastructure.ADORepositories;
using Vueling.Build.DB.Infrastructure.Configuration;
using Vueling.Build.Impl.ServiceLibrary;

namespace TFSBuildExtensions.PlatformSettings
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public class GetServerSettings : BaseCodeActivity
    {
        private BuildPlatformSettingsService buildPlatformSettingsService;
        private BuildInfrastructureConfiguration buildInfrastructureConfiguration;
        private GetBuildDataRepository getBuildDataRepository;
        private GetServerDataRepository getServerDataRepository;

        [Description("Server where an application is deployed.")]
        [RequiredArgument]
        public InArgument<string> Server { get; set; }

        [Description("Properties")]
        [RequiredArgument]
        public OutArgument<Dictionary<string, string>> Properties { get; set; }

        protected override void InternalExecute()
        {
            string serverName = this.Server.Get(this.ActivityContext);

            Dictionary<string, string> properties = null;

            if (buildPlatformSettingsService == null)
            {
                buildInfrastructureConfiguration = new BuildInfrastructureConfiguration();
                getBuildDataRepository = new GetBuildDataRepository(buildInfrastructureConfiguration);
                getServerDataRepository = new GetServerDataRepository(buildInfrastructureConfiguration);

                buildPlatformSettingsService = new BuildPlatformSettingsService(getBuildDataRepository, getServerDataRepository);
            }

            var server = buildPlatformSettingsService.GetServerSettings(serverName);

            if (server == null)
            {
                this.LogBuildWarning(String.Format("Server {0} does not exists in database, default iis, sql and ftp values will be assigned.", serverName));

                properties = new Dictionary<string, string>();

                properties.Add("server.iis.ip", serverName);
                properties.Add("server.iis.static.ip", serverName);
                properties.Add("server.iis.cms.front", serverName);
                properties.Add("server.iis.deployuser", @".\tfsbuild");
                properties.Add("server.iis.deploypassword", "Bu1ld1ng");
                properties.Add("server.iis.backup.dir", @"\\${server.iis.ip}\c$\BackupWeb\");
                properties.Add("server.iis.remote.deploy.dir", @"\\${server.iis.ip}\c$\Repositorio_Web\");
                properties.Add("server.iis.local.deploy.dir", @"C:\Repositorio_Web\");
                properties.Add("server.iis.remote.deploy.static.dir", @"\\${server.iis.static.ip}\c$\Repositorio_Web\");
                properties.Add("server.iis.remote.deploy.config.dir", @"c$\Repositorio_Web\");
                properties.Add("server.iis.cms.remote.deploy.config.dir", @"c$\Repositorio_Web\");
                properties.Add("server.iis.website", "Default Web Site");
                properties.Add("server.sql.ip", serverName);
                properties.Add("server.sql.port", "1433");
                properties.Add("server.sql.deployuser", "l.Hudson");
                properties.Add("server.sql.deploypassword", "Hudson_1804_L");
                properties.Add("server.sql.allowdrops", "True");
                properties.Add("server.ftp.ip", serverName);
                properties.Add("server.ftp.user", "tfsbuild");
                properties.Add("server.ftp.password", "Bu1ld1ng");

                properties.Add("website.id", "-1");
                properties.Add("website.name", "Default Web Site");
                properties.Add("website.physicalpath", "C:\\Repositorio_Web");
                properties.Add("website.applicationpool", "DefaultAppPool");
                properties.Add("website.bindings", "http");
                properties.Add("website.enabledprotocols", "http");
                properties.Add("website.preloadenabled", "true");

                properties.Add("server.local.backup.dir", "C:\\Repositorio_Web_Backup");
            }
            else
            {
                var mapper = MappingFromExternalDTOFactory.GetFor(EnumExternalDTOToLocalDTO.ServerProperties);

                properties = mapper.Get<Vueling.Build.Contracts.ServiceLibrary.DTO.Server, Dictionary<string, string>>(server);
            }

            this.LogBuildMessage("Server " + serverName + " properties loaded successfully.");
            Properties.Set(this.ActivityContext, properties);
        }
    }
}