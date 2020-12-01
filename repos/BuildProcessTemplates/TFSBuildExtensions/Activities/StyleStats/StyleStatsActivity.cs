using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.StyleStats;
using Vueling.Activities.Impl.ServiceLibrary.StyleStats;

namespace TFSBuildExtensions.StyleStats
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class StyleStatsActivity : BaseCodeActivity
    {
        [Description("SourcesDirectory")]
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [Description("BinariesDirectory")]
        [RequiredArgument]
        public InArgument<string> BinariesDirectory { get; set; }

        [Description("FullPathStyleStatsResults")]
        [RequiredArgument]
        public InArgument<string> FullPathStyleStatsResults { get; set; }

        [Description("FullPathJsonStyleStatsThreshold")]
        [RequiredArgument]
        public InArgument<string> FullPathJsonStyleStatsThreshold { get; set; }

        [Description("Excluded files pattern")]
        [RequiredArgument]
        public InArgument<string[]> ExcludedFilesPattern { get; set; }

        [Description("Result")]
        [RequiredArgument]
        public InOutArgument<int> Result { get; set; }

        protected override void InternalExecute()
        {
            IValidateStyleStatsMetrics validateStyleStatsMetricsService = new ValidateStyleStatsMetricsService();

            validateStyleStatsMetricsService.Initialize(this.SourcesDirectory.Get(this.ActivityContext), this.BinariesDirectory.Get(this.ActivityContext), this.FullPathStyleStatsResults.Get(this.ActivityContext), this.FullPathJsonStyleStatsThreshold.Get(this.ActivityContext), "*css", this.ExcludedFilesPattern.Get(this.ActivityContext));

            validateStyleStatsMetricsService.InternalExecute();

            this.Result.Set(this.ActivityContext, validateStyleStatsMetricsService.Result);

            this.InformationMessageList = validateStyleStatsMetricsService.InformationMessageList;
            this.WarningMessageList = validateStyleStatsMetricsService.WarningMessageList;
            this.ErrorMessageList = validateStyleStatsMetricsService.ErrorMessageList;
        }
    }
}
