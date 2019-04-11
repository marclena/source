using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;

namespace Vueling.XXX.WCF.WebService.UnitTest.Given_Flight_With_Available_Seats
{
    [TestClass]
    public class When_ChangeASeatReservation
    {

        private const string SUCCESS_MESSAGE_TO_CLIENT = "Seat reserved.";

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
            _ISeatAssignmentApplicationService.Expects.One.Method(x => x.ChangeSeatWithValidation(default(FlightDTO), default(SeatDTO), default(SeatDTO))).WithAnyArguments().WillReturn(true);
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WCF.WebService")]
        public void Then_Returns_Success_Message_If_Params_Are_Ok()
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
            Assert.AreEqual(SUCCESS_MESSAGE_TO_CLIENT, actual);

        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WCF.WebService")]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_Thrown_An_Exception_If_flighIdentifier_Param_Is_empty()
        {

            //ARRANGE
            string flighIdentifier = string.Empty;
            DateTime departureTime = DateTime.Now.AddHours(3);
            int currentRowOfSeat = 2;
            string currentColumOfSeat = "B";
            int newRowOfSeat = 2;

            //ACT
            Sut.ChangeASeatReservation(flighIdentifier, departureTime, currentRowOfSeat, currentColumOfSeat, newRowOfSeat, currentColumOfSeat);

            //ASSERT

        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.WCF.WebService")]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_Thrown_An_Exception_If_Row_Param_Is_Negative()
        {

            //ARRANGE
            string flighIdentifier = "FN1";
            DateTime departureTime = DateTime.Now.AddHours(3);
            int currentRowOfSeat = -1;
            string currentColumOfSeat = "B";
            int newRowOfSeat = 2;

            //ACT
            Sut.ChangeASeatReservation(flighIdentifier, departureTime, currentRowOfSeat, currentColumOfSeat, newRowOfSeat, currentColumOfSeat);

            //ASSERT

        }

    }

}
