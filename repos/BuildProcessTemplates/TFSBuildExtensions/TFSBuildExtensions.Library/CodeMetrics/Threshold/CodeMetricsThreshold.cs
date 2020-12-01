using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TFSBuildExtensions.Library.CodeMetrics.Threshold
{
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class CodeMetricsThreshold
    {
        private static System.Xml.Serialization.XmlSerializer serializer = null;

        [XmlElement("Assembly")]
        public LevelMetricsThreshold Assembly { get; set; }

        [XmlElement("Namespace")]
        public LevelMetricsThreshold Namespace { get; set; }

        [XmlElement("Type")]
        public LevelMetricsThreshold Type { get; set; }

        [XmlElement("Member")]
        public LevelMetricsThreshold Member { get; set; }

        public static CodeMetricsThreshold LoadFromFile(string fileName)
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
                return CodeMetricsThreshold.Deserialize(xmlString);
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

        public static CodeMetricsThreshold Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((CodeMetricsThreshold)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
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
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(CodeMetricsThreshold));
                }
                return serializer;
            }
        }
    }
}
