using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TFSBuildExtensions.Library.CodeMetrics.Report
{
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class CodeMetricsReport
    {
        private static System.Xml.Serialization.XmlSerializer serializer = null;

        public CodeMetricsReport()
        {
            this.Targets = new List<Target>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Target", IsNullable = false)]
        public List<Target> Targets { get; set; }

        [XmlIgnore]
        public Build Build { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Version { get; set; }

        public static CodeMetricsReport LoadFromFile(string fileName)
        {
            System.IO.FileStream file = null;
            System.IO.StreamReader sr = null;
            try
            {
                file = new System.IO.FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new System.IO.StreamReader(file);
                string xmlString = sr.ReadToEnd();
                sr.Close();
                file.Close();

                if (String.IsNullOrEmpty(xmlString))
                {
                    return null;
                }
                else
                {
                    return CodeMetricsReport.Deserialize(xmlString);
                }
            }
            finally
            {
                if ((file != null))
                {
                    file.Dispose();
                }
                if ((sr != null))
                {
                    sr.Dispose();
                }
            }
        }

        public static CodeMetricsReport Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((CodeMetricsReport)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
            }
            finally
            {
                if ((stringReader != null))
                {
                    stringReader.Dispose();
                }
            }
        }

        private static System.Xml.Serialization.XmlSerializer Serializer
        {
            get
            {
                if ((serializer == null))
                {
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(CodeMetricsReport));
                }
                return serializer;
            }
        }
    }
}
