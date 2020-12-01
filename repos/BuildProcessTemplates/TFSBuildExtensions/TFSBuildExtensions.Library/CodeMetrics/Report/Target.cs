using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Target
    {
        public Target()
        {
            this.Modules = new List<Module>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Module", IsNullable = false)]
        public List<Module> Modules { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
    }
}
