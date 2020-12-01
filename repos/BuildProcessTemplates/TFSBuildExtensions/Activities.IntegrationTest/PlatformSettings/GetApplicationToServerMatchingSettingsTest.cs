using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Linq;

namespace Activities.IntegrationTest.PlatformSettings
{
    [TestClass]
    public class GetApplicationToServerMatchingSettingsTest
    {
        [TestMethod]
        public void GetApplicationToServerMatchingSettingsDefault()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetFunctionalComponentSettings()
            {
                BuildName = "Vueling.Build.PRO.64"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsTrue(output.First().Value != null);
        }
    }
}
