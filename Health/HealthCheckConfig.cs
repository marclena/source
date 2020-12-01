using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC.HealthCheck.NET
{
    public class HealthCheckConfig
    {
        public string type { get; set; }
        public string name { get; set; }

        public string host { get; set; }
        public int port { get; set; }
        public string connString { get; set; }
        public string query { get; set; }
        public string uri { get; set; }
    }
}
