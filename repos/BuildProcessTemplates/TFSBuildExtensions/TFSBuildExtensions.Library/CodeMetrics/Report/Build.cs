using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Build
    {
        public Build()
        {
            this.Metrics = new List<Metric>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metric", IsNullable = false)]
        public List<Metric> Metrics { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
    }
}
