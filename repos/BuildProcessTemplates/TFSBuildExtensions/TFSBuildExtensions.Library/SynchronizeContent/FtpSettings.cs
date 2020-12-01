using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.SynchronizeContent
{
    public class FtpSettings
    {
        public string ServerAddress { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string StartingPath { get; set; }
        public bool EnableSSL { get; internal set; }
        public FtpSyncTypeEnum FtpSyncType { get; set; }
        public bool ShowHiddenFiles { get; set; }
    }
}
