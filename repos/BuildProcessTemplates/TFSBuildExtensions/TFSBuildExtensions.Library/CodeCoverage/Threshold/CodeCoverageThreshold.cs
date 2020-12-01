using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TFSBuildExtensions.Library.CodeCoverage.Threshold
{
    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class CodeCoverageThreshold
    {
        private static System.Xml.Serialization.XmlSerializer serializer = null;

        [XmlElement("Build")]
        public LevelCoverageThreshold Build { get; set; }

        [XmlElement("Assembly")]
        public LevelCoverageThreshold Assembly { get; set; }

        [XmlElement("Namespace")]
        public LevelCoverageThreshold Namespace { get; set; }

        [XmlElement("Type")]
        public LevelCoverageThreshold Type { get; set; }

        [XmlElement("Member")]
        public LevelCoverageThreshold Member { get; set; }

        public static CodeCoverageThreshold LoadFromFile(string fileName)
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
                return CodeCoverageThreshold.Deserialize(xmlString);
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

        public static CodeCoverageThreshold Deserialize(string xml)
        {
            System.IO.StringReader stringReader = null;
            try
            {
                stringReader = new System.IO.StringReader(xml);
                return ((CodeCoverageThreshold)(Serializer.Deserialize(System.Xml.XmlReader.Create(stringReader))));
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
                    serializer = new System.Xml.Serialization.XmlSerializer(typeof(CodeCoverageThreshold));
                }
                return serializer;
            }
        }
    }
}
