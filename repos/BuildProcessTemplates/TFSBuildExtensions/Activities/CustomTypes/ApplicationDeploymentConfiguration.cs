using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.CustomTypes
{
    public class ApplicationDeploymentConfiguration
    {
        [DisplayName("ApplicationName")]
        [Category("Properties")]
        public string ApplicationName { get; set; }

        [DisplayName("StagingSiteName")]
        [Category("Properties")]
        public string StagingSiteName { get; set; }

        [DisplayName("ProductionSiteName")]
        [Category("Properties")]
        public string ProductionSiteName { get; set; }

        [DisplayName("Server")]
        [Category("Properties")]
        public string Server { get; set; }

        [DisplayName("StagingLoadBalancingManagement")]
        [Category("Properties")]
        public string StagingLoadBalancingManagement { get; set; }

        [DisplayName("ProductionLoadBalancingManagement")]
        [Category("Properties")]
        public string ProductionLoadBalancingManagement { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(ApplicationName) ? "New" : ApplicationName;
        }
    }
}
