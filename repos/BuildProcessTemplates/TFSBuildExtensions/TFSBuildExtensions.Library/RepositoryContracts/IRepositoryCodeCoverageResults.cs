using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.Library.RepositoryContracts
{
    public interface IRepositoryCodeCoverageResults
    {
        bool addBuildCodeCoverageResult(string buildDefinition, string buildName, decimal codeCoverageIndex);
        bool addApplicationCodeCoverageResult(string buildName, string applicationName, decimal codeCoverageIndex);
    }
}
