using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.WebUI.Controllers;
using NMock;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using System.Web;
using System.Web.Mvc;
using Vueling.XXX.WebUI.Models;
using Vueling.XXX.WebUI.UnitTest.Helpers;
using System.Globalization;

namespace Vueling.XXX.WebUI.UnitTest.Given_Flight_With_Available_Seats.And_Params_Are_Ok
{
    [TestClass]
    public class When_ReserveSeat
    {

        private const string SUCCESS_MESSAGE_TO_CLIENT = "Seat reserved.";

        #region .: given scenario :.

        static string flighIdentifier = "FN1";
        static DateTime departureTime = DateTime.Now.AddHours(3);
        static int rowOfSeatToAssign = 2;
        static string columOfSeatToAssign = "B";
        static FlightDTO flight = new FlightDTO(flighIdentifier, departureTime);
        static SeatDTO seat = new SeatDTO(rowOfSeatToAssign.ToString(CultureInfo.InvariantCulture), columOfSeatToAssign);

        static HomeController Sut;
        static Mock<ISeatAssignmentApplicationService> _ISeatAssignmentApplicationServiceMocked;
        static Mock<HttpContextBase> _httpcontextMocked;
        static Mock<Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration> _IUIBaseBackOfficeLibraryConfigurationMocked;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            MockFactory mockFactory = new MockFactory();

            _ISeatAssignmentApplicationServiceMocked = mockFactory.CreateMock<ISeatAssignmentApplicationService>();
            _httpcontextMocked = HttpContextBaseMockFactory.GetContext(mockFactory, flight, seat);
            _IUIBaseBackOfficeLibraryConfigurationMocked = mockFactory.CreateMock<Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration>();

            //INITIALIZED SERVICE TO BE TESTED
            _IUIBaseBackOfficeLibraryConfigurationMocked.Expects.One.GetProperty<string>(x => x.StaticContentBackUrlBase).WillReturn(string.Empty);
            _IUIBaseBackOfficeLibraryConfigurationMocked.Expects.One.GetProperty<string>(x => x.StaticContentBackUrlBaseJs).WillReturn(string.Empty);

            Sut = new HomeController(_IUIBaseBackOfficeLibraryConfigurationMocked.MockObject,
                _ISeatAssignmentApplicationServiceMocked.MockObject, _httpcontextMocked.MockObject);

            //COMPLETE SCENARIO
            _ISeatAssignmentApplicationServiceMocked.Expects.One.Method(x => x.AssignSeatWithValidation(flight, seat)).WithAnyArguments().WillReturn(true);
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WebUI")]
        public void Then_Returns_Success_Message_If_Params_Are_Ok()
        {

            //ARRANGE

            //ACT
            ViewResult actual = (ViewResult)Sut.Index();

            //ASSERT
            IndexViewModel indexViewModel = new IndexViewModel();
            Assert.IsInstanceOfType(actual.Model, indexViewModel.GetType());
            indexViewModel = (IndexViewModel)actual.Model;
            Assert.AreEqual(SUCCESS_MESSAGE_TO_CLIENT, indexViewModel.Message);

        }

    }
}
