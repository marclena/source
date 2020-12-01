using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.DTO
{
    [Serializable]
    public class VirtualDirectory
    {
        private int id;
        private string name = "";
        private string virtualDirectoryType = "";
        private string applicationPool = "DefaultAppPool";
        private string physicalPath = @"C:\Repositorio_Web";
        private string enabledProtocols = "http";
        private bool preLoadEnabled = true;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string Name { 
            get { return name; }
            set { name = value; }
        }

        public string VirtualDirectoryType
        {
            get { return virtualDirectoryType; }
            set { virtualDirectoryType = value; }
        }

        public string ApplicationPool
        {
            get { return applicationPool; }
            set { applicationPool = value; }
        }

        public string PhysicalPath {
            get { return physicalPath; }
            set { physicalPath = value; }
        }

        public string EnabledProtocols {
            get { return enabledProtocols; }
            set { enabledProtocols = value; }
        }

        public bool PreLoadEnabled {
            get { return preLoadEnabled; }
            set { preLoadEnabled = value; }
        }        
    }
}
