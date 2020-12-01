using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Impl.ServiceLibrary.Database;

namespace TFSBuildExtensions.Database
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class SQLCodeAnalysisActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> PathScripFiles { get; set; }

        public OutArgument<double> Complexity { get; set; }

        public OutArgument<int> Result { get; set; }

        protected override void InternalExecute()
        {
            var iSQLCodeAnalysisService = new SQLCodeAnalysisService();

            iSQLCodeAnalysisService.Initialize(this.PathScripFiles.Get(this.ActivityContext));

            iSQLCodeAnalysisService.InternalExecute();

            this.InformationMessageList = iSQLCodeAnalysisService.InformationMessageList;
            this.WarningMessageList = iSQLCodeAnalysisService.WarningMessageList;
            this.ErrorMessageList = iSQLCodeAnalysisService.ErrorMessageList;

            this.Result.Set(this.ActivityContext, iSQLCodeAnalysisService.ErrorMessageList.Count);
        }
    }
}
