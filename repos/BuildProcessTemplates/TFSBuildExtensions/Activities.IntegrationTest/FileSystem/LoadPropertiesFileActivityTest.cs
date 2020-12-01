using System;
using System.Activities;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.FileSystem
{
    [TestClass]
    public class LoadPropertiesFileActivityTest
    {
        [TestMethod]
        public void Should_return_standard_Properties_Dictionary()
        {
            var target = new TFSBuildExtensions.FileSystem.LoadPropertiesFileActivity()
            {
                FilePath = @"c:\192.168.15.17.iis.properties",
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(16, ((Dictionary<string,string>)output["Properties"]).Count());
        }
    }
}
