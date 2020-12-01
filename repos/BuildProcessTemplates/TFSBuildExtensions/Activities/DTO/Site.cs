using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.DTO
{
    [Serializable]
    public class Site
    {
        private int id;
        private string applicationPool = "DefaultAppPool";
        private string bindings = "";
        private string name = "Default Web Site";
        private string physicalPath = @"C:\Repositorio_Web";
        private bool preLoadEnabled = true;
        private string enabledProtocols = "http";

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string ApplicationPool { 
            get { return applicationPool; } 
            set { applicationPool = value; } 
        }

        public string Bindings {
            get { return bindings; }
            set { bindings = value; }
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string PhysicalPath {
            get { return physicalPath; }
            set { physicalPath = value; }
        }

        public bool PreloadEnabled {
            get { return preLoadEnabled; }
            set { preLoadEnabled = value; }
        }

        public string EnabledProtocols {
            get { return enabledProtocols; }
            set { enabledProtocols = value; }
        }
    }
}
