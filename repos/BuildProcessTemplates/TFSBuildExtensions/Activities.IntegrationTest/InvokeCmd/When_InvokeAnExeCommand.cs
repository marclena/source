using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.IntegrationTest.InvokeCmd
{
    [TestClass]
    public class When_InvokeAnExeCommand
    {
        [TestMethod]
        public void Then_ListOfFolderIsObtained()
        {
            var target = new TFSBuildExtensions.InvokeCmd.InvokeProcessAsync
            {
                FileName = "tf.exe",
                Arguments = "/?"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

        }
    }
}
