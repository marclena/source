using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Namespace
    {
        public Namespace()
        {
            this.Members = new List<Member>();
            this.Types = new List<Type>();
            this.Metrics = new List<Metric>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metric", IsNullable = false)]
        public List<Metric> Metrics { get; set; }
       

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Type", IsNullable = false)]
        public List<Type> Types  { get; set; }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Member", IsNullable = false)]
        public List<Member> Members { get; set; }
     
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
    }
}
