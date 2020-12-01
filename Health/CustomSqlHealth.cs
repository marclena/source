using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HealthChecks.SqlServer;
namespace ATC.HealthCheck.NET
{
    public class CustomSQLHealth : SqlServerHealthCheck
    {
       
        public string HealthCheckName { get; set; }

        public CustomSQLHealth(string  connection,string sql):base(connection,sql)
        {
            
        }
            


    }
}
