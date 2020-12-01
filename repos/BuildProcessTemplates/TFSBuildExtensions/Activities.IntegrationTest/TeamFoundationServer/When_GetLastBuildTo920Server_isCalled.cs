using System;
using System.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.TeamFoundationServer
{
    [TestClass]
    public class When_GetLastBuildTo920Server_isCalled
    {
        [TestMethod]
        public void Then_Return_Snapshot_To_Deploy_To_All_platform()
        {
            var target = new TFSBuildExtensions.TeamFoundationServer.GetLastBuildTo920Server
            {
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNotNull(output["CurrentSnapshot920Server"]);
        }
    }
}
