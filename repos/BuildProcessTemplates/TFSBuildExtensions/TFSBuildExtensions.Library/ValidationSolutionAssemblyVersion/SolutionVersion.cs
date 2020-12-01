using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion
{
    [Serializable]
    public class SolutionVersion
    {
        public SolutionVersion()
        {
            this.Assemblies = new List<string>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Name", IsNullable = false)]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Assemblies", IsNullable = false)]
        public List<string> Assemblies { get; set; }
    }
}
