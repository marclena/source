using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Activities;
using Microsoft.TeamFoundation.Build.Workflow.Activities;

namespace Activities.IntegrationTest.NPM
{
    [TestClass]
    public class RunNPMCommandsActivityTest
    {
        [TestMethod]
        public void RunNPMUpdate()
        {
            var target = new TFSBuildExtensions.NPM.RunNPMCommandsActivity
            {
                SourcesDirectory = Environment.CurrentDirectory
            };

            var parameters = new Dictionary<string, object> 
            { { "Commands", new string[] {"update", "run stylestats"} } 
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.IsNotNull(output);
        }
    }
}
