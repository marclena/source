using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activities.IntegrationTest.ConfigurationSetupEnvironment
{
    [TestClass]
    public class ConfigureRabbitMQActivityTest
    {
        [TestMethod]
        public void configureRabbitINTSuccessfully()
        {
            var target = new TFSBuildExtensions.ConfigurationSetupEnvironment.ConfigureRabbitMQActivity
            {
                ConfigurationPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Activities.IntegrationTest\fake\Vueling.Configuration.Config\global.config"),
                Environment = "INT"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNotNull(output);
        }
    }
}
