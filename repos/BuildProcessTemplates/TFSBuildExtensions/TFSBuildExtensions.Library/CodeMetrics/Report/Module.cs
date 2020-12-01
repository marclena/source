using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Module
    {
        public Module()
        {
            this.Types = new List<Type>();
            this.Namespaces = new List<Namespace>();
            this.Metrics = new List<Metric>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metric", IsNullable = false)]
        public List<Metric> Metrics { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Namespace", IsNullable = false)]
        public List<Namespace> Namespaces  { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable = false)]
        public List<Type> Types  { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name  { get; set; }
    }
}
