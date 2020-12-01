using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.IntegrationTest.Validation
{
    [TestClass]
    public class XmlTextValidationTest
    {

        [TestMethod]
        public void ShouldBeXmlValidFile()
        {
            var target = new TFSBuildExtensions.Validation.XmlTextValidation
            {
                XmlFilePath = @"..\..\..\Activities.IntegrationTest\fake\Views\AgencyContact.xml"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(true, (int)output["Result"] == 0);
        }

        [TestMethod]
        public void ShouldBeXmlInvalidFile()
        {
            var target = new TFSBuildExtensions.Validation.XmlTextValidation
            {
                XmlFilePath = @"..\..\..\Activities.IntegrationTest\fake\Views\AgencyPuntoRegistrationConfirmEmail_View1.xml"
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.AreEqual(false, (int)output["Result"] == 0);

        }
    }
}


