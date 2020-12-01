using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TFSBuildExtensions.Library.CodeCoverage.Threshold
{
    [Serializable]
    public class LevelCoverageThreshold
    {
        [XmlElement("CodeCoveragePercentageError")]
        public string CodeCoveragePercentageError { get; set; }

        [XmlElement("CodeCoveragePercentageWarning")]
        public string CodeCoveragePercentageWarning { get; set; }

    }
}
