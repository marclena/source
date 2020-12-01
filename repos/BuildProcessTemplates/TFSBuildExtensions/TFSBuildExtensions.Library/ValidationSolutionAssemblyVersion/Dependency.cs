using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.ValidationSolutionAssemblyVersion
{
    [Serializable]
    public class Dependency
    {
        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Project", IsNullable = false)]
        public string Project { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Name", IsNullable = false)]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Version", IsNullable = false)]
        public string Version { get; set; }
    }
}
