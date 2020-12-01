using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.IntegrationTest.StyleStats
{
    [TestClass]
    public class When_BuildRun
    {
        [TestMethod]
        public void Then_StyleStatsResults_Ok()
        {
            string fullPathStyleStatsResults;
            string fullPathJsonStyleStatsThreshold;
            string sourcesDirectory;

            if (Environment.GetEnvironmentVariable("ISTEAMBUILDMACHINE") == null)
            {
                fullPathStyleStatsResults = @"..\..\Fixtures\SourcesDirectoryStaticContent\stylestats-results.json";
                fullPathJsonStyleStatsThreshold = @"..\..\Fixtures\SourcesDirectoryStaticContent\stylestats-threshold.json";
                sourcesDirectory = @"..\..\";
            }
            else
            {
                fullPathStyleStatsResults = @"..\Sources\Activities.IntegrationTest\Fixtures\SourcesDirectoryStaticContent\stylestats-results.json";
                fullPathJsonStyleStatsThreshold = @"..\Sources\Activities.IntegrationTest\Fixtures\SourcesDirectoryStaticContent\stylestats-threshold.json";
                sourcesDirectory = @"..\Sources";
            }
            var target = new TFSBuildExtensions.StyleStats.StyleStatsActivity
            {
                /*FullPathStyleStatsResults = @"C:\workspaces\tfs\Vueling\Vueling.StaticContent.Back\INT\stylestats-results.json",
                FullPathJsonStyleStatsThreshold = @"C:\workspaces\tfs\Vueling\BuildProcessTemplates\TFSBuildExtensions\SupportFiles\base-stylestats-threshold.json",
                SourcesDirectory = @"C:\workspaces\tfs\Vueling\Vueling.StaticContent.Back\INT"*/
                FullPathStyleStatsResults = fullPathStyleStatsResults,
                FullPathJsonStyleStatsThreshold = fullPathJsonStyleStatsThreshold,
                SourcesDirectory = sourcesDirectory
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();
        }
    }
}
