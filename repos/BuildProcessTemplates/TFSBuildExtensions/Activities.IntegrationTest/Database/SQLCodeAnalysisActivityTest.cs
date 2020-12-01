using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;

namespace Activities.IntegrationTest.Database
{
    [TestClass]
    public class SQLCodeAnalysisActivityTest
    {
        [TestMethod]
        [DeploymentItem("fake", "fake")]
        public void Test_SQL_Files_OK()
        {
            var target = new TFSBuildExtensions.Database.SQLCodeAnalysisActivity
            {
                PathScripFiles = Environment.CurrentDirectory + @"\fake\SQL\OK"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNull(output["WarningIssues"]);
        }

        [TestMethod]
        [DeploymentItem("fake", "fake")]
        public void Test_SQL_Files_KO()
        {
            var target = new TFSBuildExtensions.Database.SQLCodeAnalysisActivity
            {
                PathScripFiles = Environment.CurrentDirectory + @"\fake\SQL\KO"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNotNull(output["WarningIssues"]);
        }
    }
}
