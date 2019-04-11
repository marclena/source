using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.WebUI.Controllers;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Vueling.XXX.WebUI.Models;
using System.Collections.Specialized;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.WebUI.UnitTest
{
    [TestClass]
    public class When_HomeController_is_called
    {

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            _mockfactory = MockFactory();
            exampleDependencyObjectUsedMock = _mockfactory.CreateMock<IExampleDependencyObjectInjected>();
            configurationMock = _mockfactory.CreateMock<Vueling.XXX.WebUI.Configuration.IConfiguration>();
            httpcontextMock = MockHttpContext();

        }

        public static HomeController Sut { get; set; }
        public static Mock<IExampleDependencyObjectInjected> exampleDependencyObjectUsedMock { get; set; }
        public static Mock<Vueling.XXX.WebUI.Configuration.IConfiguration> configurationMock { get; set; }
        public static HttpContextBase httpcontextMock { get; set; }
        private static MockFactory _mockfactory { get; set; }

        [TestMethod]
        [Description("Given_logged_into_webui.When_HomeController_is_called.then_populate_correctly_the_message")]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WebUI")]
        public void then_populate_correctly_the_message_index_action()
        {
            //ARRANGE
            ExampleDataTransferObject exampleDataTransferObject = new ExampleDataTransferObject();
            List<int> parameters = new List<int>();
            parameters.Add(1);
            parameters.Add(2);
            parameters.Add(3);
            exampleDataTransferObject.paramList = parameters;

            configurationMock.Expects.One.GetProperty(v => v.DataString).WillReturn("value");
            exampleDependencyObjectUsedMock.Expects.One.Method(x => x.StoreValidPositionWithReturnedMessage(exampleDataTransferObject)).WithAnyArguments().WillReturn("message from ExampleDependencyObjectInjected");


            //ACT
            Sut = new HomeController(exampleDependencyObjectUsedMock.MockObject, configurationMock.MockObject, httpcontextMock);
            ViewResult actual = (ViewResult)Sut.Index();

            //ASSERT
            IndexViewModel indexViewModel = new IndexViewModel();
            Assert.IsInstanceOfType(actual.Model, indexViewModel.GetType());
            indexViewModel = (IndexViewModel) actual.Model;
            Assert.AreEqual("message from ExampleDependencyObjectInjected and with datastring from webui configuration with value value", indexViewModel.Message);

        }

        private static MockFactory MockFactory()
        {

            MockFactory mockFactory;

            if (_mockfactory == null) mockFactory = new MockFactory();
            else mockFactory = _mockfactory;

            return mockFactory;

        }

        private static HttpContextBase MockHttpContext()
        {

            NameValueCollection form = new NameValueCollection();
            form.Add("first", "1");
            form.Add("second", "2");
            form.Add("third", "3");

            Mock<HttpContextBase> context = _mockfactory.CreateMock<HttpContextBase>();
            Mock<HttpRequestBase> request = _mockfactory.CreateMock<HttpRequestBase>();
            Mock<HttpResponseBase> response = _mockfactory.CreateMock<HttpResponseBase>();

            request.Expects.AtLeastOne.GetProperty(v => v.Form).WillReturn(form);
            context.Expects.AtLeastOne.GetProperty(v => v.Request).WillReturn(request.MockObject);
            context.Expects.AtLeastOne.GetProperty(v => v.Response).WillReturn(response.MockObject);

            return context.MockObject;

        }

    }
}
