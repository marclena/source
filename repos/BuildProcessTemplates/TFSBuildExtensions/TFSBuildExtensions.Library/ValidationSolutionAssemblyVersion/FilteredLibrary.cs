using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion
{
    public class FilteredLibrary
    {
        public string library { get; set; }
        public string version { get; set; }
        public List<string> allowedApplications { get; set; }
    }
}
