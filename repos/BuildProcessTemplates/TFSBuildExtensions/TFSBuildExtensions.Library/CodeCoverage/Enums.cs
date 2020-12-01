using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.CodeCoverage
{
    public static class Enums
    {
        public enum Response
        {
            Ok,
            CodeCoverageConsoleNotFound,
            CodeCoverageConsoleWarning,
            CodeCoverageConsoleError,
            DataCoverageFileNotFound,
            DataCoverageXmlFileLoadError,
            BuildCodeCoverageError,
            BuildCodeCoverageWarning,
            AssemblyCodeCoverageError,
            AssemblyCodeCoverageWarning
        };
    }
}
