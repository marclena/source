using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TFSBuildExtensions.DTO
{
    [Serializable]
    [XmlRoot(ElementName = "PlatformSettings")]
    public class PlatformSettings
    {
        [XmlElement(ElementName = "Site")]
        public Site Site { get; set; }

        [XmlElement(ElementName = "ApplicationPool")]
        public ApplicationPool ApplicationPool { get; set; }
        
        [XmlElement(ElementName = "VirtualDirectory")]
        public VirtualDirectory VirtualDirectory { get; set; }
    }
}
