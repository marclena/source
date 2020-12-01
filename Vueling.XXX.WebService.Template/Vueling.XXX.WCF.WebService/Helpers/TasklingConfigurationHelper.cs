using ATC.Taskling.Client.Contracts.ServiceLibrary.Tasks.Configuration;

namespace Vueling.XXX.WCF.WebService.Helpers
{
    public static class TasklingConfigurationHelper
    {
        public static TaskConfiguration GetTaskConfiguration(
            string ApplicationName, string TaskName, int concurrencyLimit)
        {
            var taskConfiguration = new TaskConfiguration(ApplicationName, TaskName);

            taskConfiguration.ConcurrencyLimit = concurrencyLimit;
            taskConfiguration.MaxBlocksToGenerate = 5000;

            return taskConfiguration;
        }
    }
}