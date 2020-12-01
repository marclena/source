using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.Network;
namespace ATC.HealthCheck.NET
{
    public class CustomPingHealth : PingHealthCheck
    {
       
        public string HealthCheckName { get; set; }

        public CustomPingHealth(PingHealthCheckOptions options):base(options)
        {
            
        }
            


    }
}
