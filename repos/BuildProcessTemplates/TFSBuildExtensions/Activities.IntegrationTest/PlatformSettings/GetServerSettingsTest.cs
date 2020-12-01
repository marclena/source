using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activities.IntegrationTest.PlatformSettings
{
    [TestClass]
    public class GetServerSettingsTest
    {
        [TestMethod]
        public void getServerSettings_wbcnvuepretfs02()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "wbcnvuepretfs02"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.iis.ip"].ToString(), "192.168.22.25");
        }

        [TestMethod]
        public void getServerSettings_noserver()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "noserver"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.iis.ip"].ToString(), "noserver");
        }

        [TestMethod]
        public void getServerSettings_int64()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "wbcnvueintstatic"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.ftp.ip"].ToString(), "10.218.4.227");
        }

        [TestMethod]
        public void getServerSettings_wbcnvueintstaticback()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "wbcnvueintstaticback"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.ftp.ip"].ToString(), "10.218.4.195");
        }

        [TestMethod]
        public void getServerSettings_wbcnvuemockups()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "wbcnvuemockups"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.ftp.ip"].ToString(), "mockups.vueling.com");
        }

        [TestMethod]
        public void getServerSettings_from_IP()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "192.168.22.3"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.ftp.ip"].ToString(), "192.168.22.3");
        }

        [TestMethod]
        public void getServerSettings_wbcnvueprethor()
        {
            var target = new TFSBuildExtensions.PlatformSettings.GetServerSettings
            {
                Server = "wbcnvueprethor"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Dictionary<string, string> properties = (Dictionary<string, string>)output["Properties"];

            Assert.AreEqual(properties["server.ftp.ip"].ToString(), "192.168.22.3");
        }
    }
}
