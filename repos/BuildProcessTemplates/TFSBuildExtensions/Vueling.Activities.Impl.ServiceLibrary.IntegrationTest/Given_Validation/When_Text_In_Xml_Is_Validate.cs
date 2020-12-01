using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.Activities.Impl.ServiceLibrary.Validation;
using System.IO;

namespace Vueling.Activities.Impl.ServiceLibrary.IntegrationTest.Given_Validation
{
    [TestClass]
    public class When_Text_In_Xml_Is_Validate
    {
        [TestMethod]
        public void Then_Text_Is_Not_Valid()
        {
            XmlTextValidationService xmlValidate = new XmlTextValidationService();

            xmlValidate.Initialize(Path.Combine(@"..\..\..\Vueling.Activities.Impl.ServiceLibrary.IntegrationTest\fake\xml\Views\", @"AgencyPuntoRegistrationConfirmEmail_View1.xml"));

            xmlValidate.InternalExecute();

            Assert.IsTrue(xmlValidate.Result == 1 && xmlValidate.ErrorMessageList.Count > 0);
        }


        [TestMethod]
        public void Then_Text_Is_Valid()
        {
            XmlTextValidationService xmlValidate = new XmlTextValidationService();

            xmlValidate.Initialize(Path.Combine(@"..\..\..\Vueling.Activities.Impl.ServiceLibrary.IntegrationTest\fake\xml\Views\", @"AgencyContact.xml"));

            xmlValidate.InternalExecute();

            Assert.IsTrue(xmlValidate.Result == 0);
        }

        [TestMethod]
        public void Then_XML_Is_Ignored()
        {
            XmlTextValidationService xmlValidate = new XmlTextValidationService();

            xmlValidate.Initialize(Path.Combine(@"..\..\..\Vueling.Activities.Impl.ServiceLibrary.IntegrationTest\fake\xml\", @"IgnoreFile.xml"));

            xmlValidate.InternalExecute();

            Assert.IsTrue(xmlValidate.ErrorMessageList.Count == 0 && xmlValidate.InformationMessageList.Count == 0);
        }
    }
}

