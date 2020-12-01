using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.SynchronizeContent
{
    public class TargetSettings
    {
        public string RootPath { get; set; }
        public SourceTypeEnum SourceType { get; set; }
        public FtpSettings FtpSettings { get; set; }
    }
}
