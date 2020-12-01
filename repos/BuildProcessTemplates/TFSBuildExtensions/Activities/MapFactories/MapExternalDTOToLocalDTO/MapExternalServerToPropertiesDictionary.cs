using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO
{
    internal class MapExternalServerToPropertiesDictionary : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Vueling.Build.Contracts.ServiceLibrary.DTO.Server;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("server.iis.ip", entity.server_ip);
            properties.Add("server.iis.static.ip", entity.server_ip);
            properties.Add("server.iis.cms.front", entity.server_ip);
            properties.Add("server.iis.deployuser", entity.server_iis_deployuser);
            properties.Add("server.iis.deploypassword", entity.server_iis_deploypassword);
            properties.Add("server.iis.backup.dir", @"\\${server.iis.ip}\c$\BackupWeb\");
            properties.Add("server.iis.remote.deploy.dir", entity.server_iis_remote_deploy_dir);
            properties.Add("server.iis.local.deploy.dir", entity.server_iis_local_deploy_dir);
            properties.Add("server.iis.remote.deploy.static.dir", entity.server_iis_remote_deploy_dir);
            properties.Add("server.iis.remote.deploy.config.dir", entity.server_iis_remote_deploy_config_dir);
            properties.Add("server.iis.cms.remote.deploy.config.dir", entity.server_iis_remote_deploy_config_dir);
            properties.Add("server.iis.website", entity.server_iis_website);
            properties.Add("server.sql.ip", entity.server_sql_ip);
            properties.Add("server.sql.port", entity.server_sql_port);
            properties.Add("server.sql.deployuser", entity.server_sql_deployuser);
            properties.Add("server.sql.deploypassword", entity.server_sql_deploypassword);
            properties.Add("server.sql.allowdrops", entity.server_sql_allowdrops.ToString());
            properties.Add("server.ftp.ip", entity.server_ftp_ip);
            properties.Add("server.ftp.user", entity.server_ftp_user);
            properties.Add("server.ftp.password", entity.server_ftp_password);

            properties.Add("website.id", entity.website.Id.ToString());
            properties.Add("website.name", entity.website.Name);
            properties.Add("website.physicalpath", entity.website.PhysicalPath);
            properties.Add("website.applicationpool", entity.website.ApplicationPool);
            properties.Add("website.bindings", entity.website.Bindings);
            properties.Add("website.enabledprotocols", entity.website.EnabledProtocols);
            properties.Add("website.preloadenabled", entity.website.PreloadEnabled.ToString());

            properties.Add("server.local.backup.dir", entity.server_local_backup_dir);

            return properties as TOutput;
        }
    }
}
