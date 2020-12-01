using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.Activities.Impl.ServiceLibrary.StyleStats;
using Vueling.Activities.Contracts.ServiceLibrary.StyleStats;
using TFSBuildExtensions.Library.StyleStats;
using System.IO;

namespace Vueling.Activities.Impl.ServiceLibrary.UnitTest.Given_StyleStats
{
    [TestClass]
    public class When_StyleStatsResults
    {
        [DeploymentItem("fake")]
        [TestInitialize]
        public void Initialize()
        {
        }
        [TestMethod]
        public void Then_StyleStats_Parse_Error()
        {
            string basepathStyleStatsThreshold;

            if (Environment.GetEnvironmentVariable("ISTEAMBUILDMACHINE") == null)
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\SupportFiles\base-stylestats-threshold.json");
            }
            else
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"base-stylestats-threshold.json");
            }

            IValidateStyleStatsMetrics validateStyleStatsMetricsService = new ValidateStyleStatsMetricsService();

            validateStyleStatsMetricsService.Initialize(Path.Combine(Directory.GetCurrentDirectory(), @"css"), Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), @"stylestatsresults"), basepathStyleStatsThreshold, "wrong.css");

            validateStyleStatsMetricsService.InternalExecute();

            Assert.AreEqual(1, validateStyleStatsMetricsService.Result);
        }


        [TestMethod]
        public void Then_StyleStats_Rules_Under_Threshold()
        {
            string basepathStyleStatsThreshold;

            if (Environment.GetEnvironmentVariable("ISTEAMBUILDMACHINE") == null)
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\SupportFiles\base-stylestats-threshold.json");
            }
            else
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"base-stylestats-threshold.json");
            }

            IValidateStyleStatsMetrics validateStyleStatsMetricsService = new ValidateStyleStatsMetricsService();

            validateStyleStatsMetricsService.Initialize(Path.Combine(Directory.GetCurrentDirectory(), @"fake\css"), Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), @"fake\stylestatsresults"), basepathStyleStatsThreshold, "foo.css");

            validateStyleStatsMetricsService.InternalExecute();

            Assert.AreEqual(0, validateStyleStatsMetricsService.Result);
        }

        [TestMethod]
        public void Then_StyleStats_Rules_Above_Threshold()
        {
            string basepathStyleStatsThreshold;

            if (Environment.GetEnvironmentVariable("ISTEAMBUILDMACHINE") == null)
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\SupportFiles\base-stylestats-threshold.json");
            }
            else
            {
                basepathStyleStatsThreshold = Path.Combine(Directory.GetCurrentDirectory(), @"base-stylestats-threshold.json");
            }

            IValidateStyleStatsMetrics validateStyleStatsMetricsService = new ValidateStyleStatsMetricsService();

            validateStyleStatsMetricsService.Initialize(Path.Combine(Directory.GetCurrentDirectory(), @"fake\css-wrong-file-above-threshold"), Directory.GetCurrentDirectory(), Path.Combine(Directory.GetCurrentDirectory(), @"fake\stylestatsresults"), basepathStyleStatsThreshold, "file-above-threshold.css");

            validateStyleStatsMetricsService.InternalExecute();

            Assert.AreEqual(1, validateStyleStatsMetricsService.Result);
        }
    }
}
