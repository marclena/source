using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC.HealthCheck.NET
{
    public class HealthCheckResponse
    {
        public string machineName { get; set; }
        public Dictionary<string,string>healthy { get; set; }
        public Dictionary<string,string> properties { get; set; }
        public string status { get; set; }
    }
}
