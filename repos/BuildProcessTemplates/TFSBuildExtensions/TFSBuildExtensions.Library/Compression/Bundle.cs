using System.IO;
using System.Xml.Serialization;

namespace TFSBuildExtensions.Library.Compression
{
    [XmlRoot("bundle", Namespace = "")]
    public class Bundle
    {
        [XmlElement("file")]
        public string[] Files { get; set; }

        [XmlAttribute("output")]
        public string OutPut { get; set; }

        [XmlAttribute("minify")]
        public bool Minify { get; set; }

        [XmlAttribute("runOnBuild")]
        public bool RunOnBuild { get; set; }
    }
}
