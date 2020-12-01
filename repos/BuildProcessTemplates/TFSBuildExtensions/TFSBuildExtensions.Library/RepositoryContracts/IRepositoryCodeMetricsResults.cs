using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSBuildExtensions.Library.CodeMetrics.Report;

namespace TFSBuildExtensions.Library.RepositoryContracts
{
    public interface IRepositoryCodeMetricsResults
    {
        bool addBuildCodeMetricsResult(Build build);
        bool addApplicationCodeMetricsResult(Module application, string buildName);
    }
}
