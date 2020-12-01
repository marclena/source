using System;
using System.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.IntegrationTest.Navitaire
{
    [TestClass]
    public class GetSnapshotsFromPoolTest
    {
        [TestMethod]
        public void GetPreviousSnapshotFrom920Pool()
        {
            var target = new TFSBuildExtensions.Navitaire.GetSnapshotsFromPool
            {
                Pool = "Vueling 920",
                FullPathVuelingNavitaireWebDeployManagerConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Navitaire.Web.Deploy.Manager.ConsoleUI\Vueling.Navitaire.Web.Deploy.Manager.ConsoleUI.exe"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            StringAssert.StartsWith(output["PreviousSnapshot"].ToString(), "SKYSALES");
        }

        [TestMethod]
        public void GetActiveSnapshotFrom920Pool()
        {
            var target = new TFSBuildExtensions.Navitaire.GetSnapshotsFromPool
            {
                Pool = "Vueling 920",
                FullPathVuelingNavitaireWebDeployManagerConsoleUI = @"D:\workspaces\tfs\BuildProcessTemplate\Vueling.Navitaire.Web.Deploy.Manager.ConsoleUI\Vueling.Navitaire.Web.Deploy.Manager.ConsoleUI.exe"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            StringAssert.StartsWith(output["ActiveSnapshot"].ToString(), "SKYSALES");
        }
    }
}
