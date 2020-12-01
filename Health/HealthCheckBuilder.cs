using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HealthChecks.SqlServer;
using HealthChecks.Redis;
using System.Text;
using System.Threading.Tasks;
using HealthChecks.Network;
using HealthChecks.Uris;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace ATC.HealthCheck.NET
{
    public static class HealthCheckBuilder
    {
        private const string ConfigDirectory = @"C:\VuelingApps\Config\meta.config";
        private const string jsonfile = "healthchecks.json";
        private static  HealthCheckConfig _healthConfig;
        private  static  IConfigurationRoot _config;
        private static  List<IHealthCheck> _checks = new List<IHealthCheck>();
        private static List<HealthCheckConfig> _configs = new List<HealthCheckConfig>();
         public static List<IHealthCheck> ApplyHealthChecksConfig(List<HealthCheckConfig> healthCheckCoçnfig)
        {
            foreach (HealthCheckConfig config in healthCheckCoçnfig)
            {
                switch (config.type)
                {
                    case "PingHealthCheck":
                        
                        _checks.Add(AddPingHealthChecks(config.host, config.port,config.name));
                    break;
                    case "SqlServerHealthCheck":
                         _checks.Add(AddSQLHealthChecks(config.connString, config.query,config.name));
                    break;
                    case "HttpHealthCheck":
                        _checks.Add(AddURIHealthChecks(config.name,config.uri));
                    break;
                    case "RedisHealthCheck":
                      _checks.Add(AddRedisHealthChecks(config.connString,config.name));
                    break;
                }
            }
            return _checks;
        }
        public static List<HealthCheckConfig> ReadFromJson(string appID)
        {
            
            XDocument xml = XDocument.Load(ConfigDirectory);
            var metaConfigElement = xml.Root.Elements("server-meta-config").SingleOrDefault();
            string jsonFileDirectory;
            jsonFileDirectory = metaConfigElement.Attribute("configDirectory").Value;
            jsonFileDirectory = jsonFileDirectory + @"\" + appID + @"\" + jsonfile;

            IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile(jsonFileDirectory, optional: true)
    .Build();

            _config = config;
            var hcSections = config
             .GetSection("HealthChecks")
             .GetChildren();
          
            foreach (IConfigurationSection section in hcSections)
            {
               
                var type = section["Type"];
                var name = section["Name"];
                
                switch (type)
                {
                    case "PingHealthCheck":
                        var host = section["Properties:host"];
                        var port = int.Parse(section["Properties:port"]);
                        _healthConfig = new HealthCheckConfig {type=type,name=name,port=port,host=host };
                        _configs.Add(_healthConfig);                       
                        break;
                    case "SqlServerHealthCheck":
                        var connstring = section["Properties:ConnString"];
                        var query = section["Properties:Query"];
                        _healthConfig = new HealthCheckConfig { type = type, name = name,connString= connstring, query = query };
                        _configs.Add(_healthConfig);
                         break;
                    case "HttpHealthCheck":
                        var uri = section["Properties:Uri"];
                        _healthConfig = new HealthCheckConfig { type = type, name = name, uri = uri };
                        _configs.Add(_healthConfig);                      
                        break;
                    case "RedisHealthCheck":
                        var connstringRedis = section["Properties:ConnString"];
                        _healthConfig = new HealthCheckConfig { type = type, name = name, connString = connstringRedis };
                        _configs.Add(_healthConfig);
                        break;
                }                       

            }
            return _configs;
        }
        private static void GetProperties(IEnumerable<IConfigurationSection> properties)
        {
           
            foreach (IConfigurationSection section in properties)
            {
               

            }






        }

        public static IHealthCheck AddPingHealthChecks(string url, int port,string name)
        {
            return (new CustomPingHealth(new PingHealthCheckOptions().AddHost(url, port)) { HealthCheckName = name });
            //return (new PingHealthCheck(new PingHealthCheckOptions().AddHost(url,port)));
            
        }
        public static IHealthCheck AddURIHealthChecks(string name,string url)
        {         
            return (new HttpHealthCheck(url) { HealthCheckName = name });         
            
        }

        public static IHealthCheck AddSQLHealthChecks(string connectionString,string query,string name)
        {
            return (new CustomSQLHealth (connectionString, query) { HealthCheckName = name });

        }

        public static IHealthCheck AddRedisHealthChecks(string redisConnection,string name)
        {
            return (new CustomRedisHealth(redisConnection) { HealthCheckName = name });
            //return (new RedisHealthCheck(redisConnection));

        }
    }
}
