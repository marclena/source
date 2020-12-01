using System;
using System.Activities;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.Database
{
    [TestClass]
    public class DeploySqlDatabaseTest
    {
        [TestMethod]
        public void ShouldDeployVueling_XXXSql2012Database()
        {
            var target = new TFSBuildExtensions.Database.DeploySqlDatabase
                             {
                                 BinariesDirectory = @"D:\workspaces\tfs\Vueling.XXX\INT\Vueling_XXX.Database\sql\release",
                                 DatabaseName = "Vueling_XXX",
                                 DatabaseServer = "10.218.4.219",
                                 DatabaseServerPort = "1433",
                                 SourcesDirectory = @"D:\workspaces\tfs\Vueling",
                                 DeployUser = "it.desarrollo",
                                 DeployPassword = "develop2"
                             };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            bool containsError = output["StdOutput"].ToString().Contains("An error occurred");
            Assert.AreEqual(false, containsError);
        }

        [TestMethod]
        public void ShouldDeploySql2010MaestrosDatabaseToINT()
        {
            var target = new TFSBuildExtensions.Database.DeploySqlDatabase
            {
                BinariesDirectory = @"D:\workspaces\tfs\Vueling.Maestros\INT\Vueling_Maestros.Database\sql\release",
                DatabaseName = "Vueling_Maestros",
                DatabaseServer = "10.218.4.219",
                DatabaseServerPort = "1433",
                SourcesDirectory = @"D:\workspaces\tfs\Vueling",
                DeployUser = "it.desarrollo",
                DeployPassword = "develop2",
                AllowDrops = "True"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            bool containsError = output["StdOutput"].ToString().Contains("An error occurred");
            Assert.AreEqual(false, containsError);
        }

        [TestMethod]
        public void ShouldDeploySql2010ResidentsDatabaseToINT()
        {
            var target = new TFSBuildExtensions.Database.DeploySqlDatabase
            {
                BinariesDirectory = @"D:\workspaces\tfs\Vueling.Residents\INT\Vueling_Residents.Database\sql\release",
                DatabaseName = "Vueling_Residents",
                DatabaseServer = "10.218.4.219",
                DatabaseServerPort = "1433",
                SourcesDirectory = @"D:\workspaces\tfs\Vueling",
                DeployUser = "it.desarrollo",
                DeployPassword = "develop2",
                AllowDrops = "True"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            bool containsError = output["StdOutput"].ToString().Contains("An error occurred");
            Assert.AreEqual(false, containsError);
        }

        [TestMethod]
        public void ShouldDeploySql2010I3QueueIIDatabaseToINT()
        {
            var target = new TFSBuildExtensions.Database.DeploySqlDatabase
            {
                BinariesDirectory = @"D:\workspaces\tfs\Vueling.I3queue\INT\Vueling_I3queueII.Database\sql\release",
                DatabaseName = "Vueling_I3queueII",
                DatabaseServer = "10.218.4.219",
                DatabaseServerPort = "1433",
                SourcesDirectory = @"D:\workspaces\tfs\Vueling",
                DeployUser = "it.desarrollo",
                DeployPassword = "develop2",
                AllowDrops = "True"            
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            bool containsError = output["StdOutput"].ToString().Contains("An error occurred");
            Assert.AreEqual(false, containsError);
        }

        [TestMethod]
        public void ShouldDeploySql2010SeatsToPRE()
        {
            var target = new TFSBuildExtensions.Database.DeploySqlDatabase
            {
                BinariesDirectory = @"D:\workspaces\tfs\Vueling.Residents\PRE\Vueling_Seats.Database\sql\release",
                DatabaseName = "Vueling_Seats",
                DatabaseServer = "10.218.4.218",
                DatabaseServerPort = "1433",
                SourcesDirectory = @"D:\workspaces\tfs\Vueling",
                DeployUser = "it.desarrollo",
                DeployPassword = "develop2",
                AllowDrops = "True"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            bool containsError = output["StdOutput"].ToString().Contains("An error occurred");
            Assert.AreEqual(false, containsError);
        }
    }
}
