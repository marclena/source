using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.StyleStats;

namespace Vueling.Activities.Contracts.ServiceLibrary.StyleStats
{
    public interface IValidateStyleStatsMetrics : IBaseActivityService
    {
        void Initialize(string sourcesDirectory, string binariesDirectory, string fullPathJsonResults, string fullPathJsonResultsThreshold, string searchPattern = "*css", string[] excludedFilesPattern = null);
    }
}
