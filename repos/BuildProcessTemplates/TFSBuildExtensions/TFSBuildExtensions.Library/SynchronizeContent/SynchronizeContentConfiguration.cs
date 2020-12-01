using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.SynchronizeContent
{
    public class SynchronizeContentConfiguration
    {
        public SourceSettings SourceSettings { get; set; }
        public TargetSettings TargetSettings { get; set; }

        public List<string> ExcludeContent { get; set; }
    }
}
