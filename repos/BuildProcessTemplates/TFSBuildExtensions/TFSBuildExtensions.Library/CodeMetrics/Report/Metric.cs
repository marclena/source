using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    public class Metric
    {
        public Metric()
        {
        }

        public Metric(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value { get; set; }
        
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name { get; set; }
    }
}
