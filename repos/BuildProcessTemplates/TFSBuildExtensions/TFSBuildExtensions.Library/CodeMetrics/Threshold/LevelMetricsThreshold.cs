using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TFSBuildExtensions.Library.CodeMetrics.Threshold
{
    [Serializable]
    public class LevelMetricsThreshold
    {
        [XmlElement("MaintainabilityIndexError")]
        public string MaintainabilityIndexError { get; set; }

        [XmlElement("CyclomaticComplexityError")]
        public string CyclomaticComplexityError { get; set; }

        [XmlElement("ClassCouplingError")]
        public string ClassCouplingError { get; set; }

        [XmlElement("DepthOfInheritanceError")]
        public string DepthOfInheritanceError { get; set; }

        [XmlElement("LinesOfCodeError")]
        public string LinesOfCodeError { get; set; }

        [XmlElement("MaintainabilityIndexWarning")]
        public string MaintainabilityIndexWarning { get; set; }

        [XmlElement("CyclomaticComplexityWarning")]
        public string CyclomaticComplexityWarning { get; set; }

        [XmlElement("ClassCouplingWarning")]
        public string ClassCouplingWarning { get; set; }

        [XmlElement("DepthOfInheritanceWarning")]
        public string DepthOfInheritanceWarning { get; set; }

        [XmlElement("LinesOfCodeWarning")]
        public string LinesOfCodeWarning { get; set; }

    }
}
