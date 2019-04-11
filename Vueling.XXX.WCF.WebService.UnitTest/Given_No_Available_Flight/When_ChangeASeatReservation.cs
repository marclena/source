using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.DB.Infrastructure.Exceptions;

namespace Vueling.XXX.WCF.WebService.UnitTest.Given_No_Available_Flight
{
    [TestClass]
    public class When_ChangeASeatReservation
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
            _ISeatAssignmentApplicationService.Expects.One.Method(x => x.ChangeSeatWithValidation(default(FlightDTO), default(SeatDTO), default(SeatDTO))).WithAnyArguments().Will(Throw.Exception(new AircraftNotFoundOnDatabaseException()));
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WCF.WebService")]
        public void Then_Returns_Error_Message_Of_application()
        {

            //ARRANGE
            string flighIdentifier = "FN1";
            DateTime departureTime = DateTime.Now.AddHours(3);
            int currentRowOfSeat = 2;
            string currentColumOfSeat = "B";
            int newRowOfSeat = 2;

            //ACT
            string actual = Sut.ChangeASeatReservation(flighIdentifier, departureTime, currentRowOfSeat, currentColumOfSeat, newRowOfSeat, currentColumOfSeat);

            //ASSERT
            Assert.AreEqual(actual, APPLICATION_ERROR_MESSAGE_TO_CLIENT);

        }

    }
}
