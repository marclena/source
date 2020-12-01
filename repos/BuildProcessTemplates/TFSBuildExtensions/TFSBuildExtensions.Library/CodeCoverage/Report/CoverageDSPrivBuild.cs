using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeCoverage.Report
{
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CoverageDSPrivBuild")]
    public class CoverageDSPrivBuild
    {
        #region private Properties

        private decimal buildCodeCoveragePercentage;

        private int linesCoveredField;

        private int linesPartiallyCoveredField;

        private int linesNotCoveredField;

        private int blocksCoveredField;

        private int blocksNotCoveredField;

        #endregion

        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal BuildCodeCoveragePercentage
        {
            get { return buildCodeCoveragePercentage; }
            set { buildCodeCoveragePercentage = value; }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesCovered
        {
            get { return linesCoveredField; }
            set { linesCoveredField = value; }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesPartiallyCovered
        {
            get { return linesPartiallyCoveredField; }
            set { linesPartiallyCoveredField = value; }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int LinesNotCovered
        {
            get { return linesNotCoveredField; }
            set { linesNotCoveredField = value; }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BlocksCovered
        {
            get { return blocksCoveredField; }
            set { blocksCoveredField = value; }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public int BlocksNotCovered
        {
            get { return blocksNotCoveredField; }
            set { blocksNotCoveredField = value; }
        }
    }
}
