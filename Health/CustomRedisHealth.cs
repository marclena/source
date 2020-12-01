using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.Network;
using HealthChecks.Redis;
namespace ATC.HealthCheck.NET
{
    public class CustomRedisHealth : RedisHealthCheck
    {
       
        public string HealthCheckName { get; set; }

        public CustomRedisHealth(string  connection):base(connection)
        {
            
        }
            


    }
}
