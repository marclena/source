using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Library.Exceptions;
using System.Globalization;

[assembly: CLSCompliant(false)]
namespace Vueling.XXX.WCF.REST.WebService.UnitTest.Given_No_Database_Connection
{
    [TestClass]
    public class When_ReserveSeat
    {

        private const string APPLICATION_ERROR_MESSAGE_TO_CLIENT = "Error reservating the seat due an application error. Seat not reserved.";

        #region .: given scenario :.

        static SeatReservationForAircraftsWebService Sut;
        static Mock<ISeatAssignmentApplicationService> _ISeatAssignmentApplicationService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            MockFactory mockFactory = new MockFactory();

            _ISeatAssignmentApplicationService = mockFactory.CreateMock<ISeatAssignmentApplicationService>();

            //INITIALIZED SERVICE TO BE TESTED
            Sut = new SeatReservationForAircraftsWebService(_ISeatAssignmentApplicationService.MockObject);

            //COMPLETE SCENARIO
            _ISeatAssignmentApplicationService.Expects.One.Method(x => x.AssignSeatWithValidation(default(FlightDTO), default(SeatDTO))).WithAnyArguments().Will(Throw.Exception(new UpdateToRepositoryException()));

        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WCF.WebService")]
        public void Then_Returns_Error_Message_Of_application()
        {

            //ARRANGE
            string flighIdentifier = "FN1";
            DateTime departureTime = DateTime.Now.AddHours(3);
            string departureTimeString = departureTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            int rowOfSeatToAssign = 2;
            string columOfSeatToAssign = "B";

            //ACT
            string actual = Sut.ReserveASeat(flighIdentifier, departureTimeString, rowOfSeatToAssign.ToString(CultureInfo.InvariantCulture), columOfSeatToAssign);

            //ASSERT
            Assert.AreEqual(actual, APPLICATION_ERROR_MESSAGE_TO_CLIENT);

        }

    }
}
