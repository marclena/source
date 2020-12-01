using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.UnitTest.Validation
{
    [TestClass]
    public class AssemblyFrameworkValidationTest
    {
        [TestMethod]
        public void ValidAssemblies()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.AssemblyFrameworkValidation
            {
                Path = @"D:\workspaces\tfs\SkySales\PRE\SkySales\bin",
                AssemblyFrameworkVersion = "v2.0.50727"
            };

            var parameters = new Dictionary<string, object>{};

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["Result"]);
        }
    }
}
