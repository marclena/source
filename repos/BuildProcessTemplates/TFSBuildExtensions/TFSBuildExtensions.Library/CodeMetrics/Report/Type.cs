using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Type
    {
        public Type()
        {
            this.Members = new List<Member>();
            this.Metrics = new List<Metric>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metric", IsNullable = false)]
        public List<Metric> Metrics { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Member", IsNullable = false)]
        public List<Member> Members { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
    }
}
