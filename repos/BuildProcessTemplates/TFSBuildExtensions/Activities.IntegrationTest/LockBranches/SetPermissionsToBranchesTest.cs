using System;
using System.Activities;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.LockBranches
{
    /// <summary>
    /// Summary description for SetPermissionsToBranchesTest
    /// </summary>
    [TestClass]
    public class SetPermissionsToBranchesTest
    {
        public SetPermissionsToBranchesTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ShouldEnablePermissionsToConfigurationProject()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.LockBranches.SetPermissionsToBranches
            {
                DirectorytoApplyPermissions = "Vueling.Configuration",
                EnvironmentBranch = "INT",
                PermissionSetting = "Allow"
            };

            var parameters = new Dictionary<string, object>
            { { "Identities", new string[] { @"[Vueling]\Contributors", @"[Vueling]\Umbraco Developers", @"[Vueling]\Project Managers" } },
              { "Permissions", new string[] { "Checkin" } }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, true);
        }
    }
}
