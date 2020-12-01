using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.LockBuilds
{
    [TestClass]
    public class When_SetPermissionBuildDefinitionTest
    {
        [TestMethod]
        public void Then_PermisionsTo_Vueling_XXX_INT_PRE_Allowed_To_Contributors()
        {
            var target = new TFSBuildExtensions.LockBuilds.SetPermissionsBuildDefinition {
                PermissionSetting = "Allow"
            };

            var parameters = new Dictionary<string, object>
            { { "Identities", new string[] { @"[Vueling]\Contributors"} },
              { "Pattern", new string[] {"Vueling.XXX.INT", "Vueling.XXX.PRE"}},
              { "Permissions", new List<int> {BuildPermissions.QueueBuilds}}
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["PermissionsAppliedSuccessfully"]);
        }

        [TestMethod]
        public void Then_PermisionsTo_Vueling_XXX_INT_PRE_Denied_To_Contributors()
        {
            var target = new TFSBuildExtensions.LockBuilds.SetPermissionsBuildDefinition {
                PermissionSetting = "Deny"
            };

            var parameters = new Dictionary<string, object>
            { { "Identities", new string[] { @"[Vueling]\Contributors"} },
              { "Pattern", new string[] {"Vueling.XXX.INT", "Vueling.XXX.PRE"}},
              { "Permissions", new List<int> {BuildPermissions.QueueBuilds}}
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["PermissionsAppliedSuccessfully"]);
        }

        [TestMethod]
        public void Then_PermisionsTo_PRO_Denied_To_Tech_Leads()
        {
            var target = new TFSBuildExtensions.LockBuilds.SetPermissionsBuildDefinition { 
                    PermissionSetting = "Deny"
            };

            var parameters = new Dictionary<string, object>
            { { "Identities", new string[] { @"[Vueling]\Tech Leads"} },
              { "Pattern", new string[] {".PRO."}},
              { "Permissions", new List<int> {BuildPermissions.QueueBuilds}}
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["PermissionsAppliedSuccessfully"]);
        }

        [TestMethod]
        public void Then_PermisionsTo_PRO_Allow_To_Tech_Leads()
        {
            var target = new TFSBuildExtensions.LockBuilds.SetPermissionsBuildDefinition
            {
                PermissionSetting = "Allow"
            };

            var parameters = new Dictionary<string, object>
            { { "Identities", new string[] { @"[Vueling]\Tech Leads"} },
              { "Pattern", new string[] {".PRO."}},
              { "Permissions", new List<int> {BuildPermissions.QueueBuilds}}
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["PermissionsAppliedSuccessfully"]);
        }

    }
}
