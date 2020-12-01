using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.IntegrationTest.PlatformSettings
{
    [TestClass]
    public class GetPlatformSettingsTest
    {
        [TestMethod]
        public void GetPlatformSettingsDefault()
        {
            Vueling.Build.Contracts.ServiceLibrary.DTO.PlatformSettings platformSettings;

            var target = new TFSBuildExtensions.PlatformSettings.GetPlatformSettings
            {
                Server = "wbcnvueintback1",
                Website = "Default Web site",
                ApplicationType = "Vueling.XXX.WebUI"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            StringAssert.StartsWith(output["PreviousSnapshot"].ToString(), "SKYSALES");
        }
    }
}
