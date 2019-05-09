using ATC.Taskling.Client.Contracts.ServiceLibrary;
using System;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.WCF.WebService.Helpers;

namespace Vueling.XXX.WCF.WebService
{
    public class AutomaticTasklingProcesses : IAutomaticTasklingProcesses
    {
        #region ..: Fields :..

        private readonly ITasklingClient _tasklingClient;
        private readonly ITasklingService _tasklingService;

        private const string WCFName = "Vueling.XXX.WCF.WebService.AutomaticTasklingProcesses";

        #endregion

        #region ..: Constructor :..

        public AutomaticTasklingProcesses(
            ITasklingClient tasklingClient,
            ITasklingService tasklingService)
        {
            _tasklingClient = tasklingClient;
            _tasklingService = tasklingService;
        }

        #endregion

        #region ..: Public Methods :..

        public void CreationBlocksExample()
        {
            var taskConfiguration = TasklingConfigurationHelper.GetTaskConfiguration(WCFName, "CreationBlocksExample", 1);

            using (var executionContext = _tasklingClient.CreateTaskExecutionContext(taskConfiguration))
            {
                try
                {
                    if (!executionContext.TryStart())
                        return;

                    _tasklingService.CreationBlocksExample(executionContext);
                }
                catch(Exception ex)
                {
                    executionContext.Error(ex.ToString(), true);
                }
            }
        }

        #endregion
    }
}
