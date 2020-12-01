using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.Release
{
    [Serializable]
    public class ReleaseUniqueIdentifier
    {
        public string source { get; set; }
        public Guid guid { get; set; }
        public List<string> ServerItems { get; set; }
    }
}
