using System;
using System.Activities;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSBuildExtensions.Validation;

namespace Activities.UnitTest.Validation
{
    [TestClass]
    public class XmlFormatValidationTest
    {
        [TestMethod]
        public void ShouldBeXmlValidFile()
        {
            var target = new TFSBuildExtensions.Validation.XmlFormatValidation
            {
                FilePathToValidate = @"D:\workspaces\tfs\Vueling.Configuration\INT\Vueling.Configuration.Config\global.config"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(true, output["IsXmlFileFormatValid"]);
        }

        [TestMethod]
        public void ShouldBeXmlInvalidFile()
        {
            var target = new TFSBuildExtensions.Validation.XmlFormatValidation
            {
                FilePathToValidate = @"D:\workspaces\tfs\BuildProcessTemplates\TFSBuildExtensions\Activities\Validation\XmlFormatValidation.cs"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(false, output["IsXmlFileFormatValid"]);

        }

        [TestMethod]
        public void ShouldBeXsltValidFile()
        {
            var target = new TFSBuildExtensions.Validation.XmlFormatValidation
            {
                FilePathToValidate = @"D:\workspaces\tfs\SkySales\INT\SkySales\Views\Base\XSLT\BaseView.xslt"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(true, output["IsXmlFileFormatValid"]);
            
        }
    }
}
