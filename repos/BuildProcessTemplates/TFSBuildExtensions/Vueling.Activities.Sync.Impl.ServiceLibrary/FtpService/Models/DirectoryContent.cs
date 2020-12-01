using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models
{
    public class DirectoryContent
    {
        public string AccessPath { get; set; }
        public bool IsDirectory { get; set; }
        public string RelativePath { get; set; }
        public string FileSize { get; set; }
        public DateTime LastWrite { get; set; }
    }
}
