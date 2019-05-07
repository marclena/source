using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

[assembly: CLSCompliant(false)]
namespace Vueling.XXX.Impl.ServiceLibrary.UnitTest.Given_TimeLimitForSeatAssigmentNotExceded
{
    [TestClass]
    public class When_AssignSeatsWithTimeLimitValidation
    {

        #region .: given scenario :.

        static ISeatAssignmentApplicationService Sut { get; set; }
        static Mock<ISeatAssignment> seatAssignmentMock { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            MockFactory mockFactory = new MockFactory();
            seatAssignmentMock = mockFactory.CreateMock<ISeatAssignment>();

            //INITIALIZED SERVICE TO BE TESTED
            Sut = new SeatAssignmentApplicationService(seatAssignmentMock.MockObject);

            //COMPLETE SCENARIO
            seatAssignmentMock.Expects.One.Method(x => x.ValidateTimeLimitBeforeFlightForAssignment(default(Aircraft))).WithAnyArguments().WillReturn(true);
            seatAssignmentMock.Expects.One.Method(x => x.Assign(default(Aircraft), default(Seat))).WithAnyArguments().WillReturn(true);

        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Impl.ServiceLibrary")]
        public void Then_AssignSeatWithValidation_Returns_True_If_SeatDTO_Is_Valid()
        {
            //ARRANGE
            FlightDTO flightWhereToAssignSeats = new FlightDTO("FN1", DateTime.Now.AddHours(3));
            SeatDTO serviceLibrarySeatDTOToBeAssigned = new SeatDTO("A", "2");

            //ACT
            bool actual = Sut.AssignSeatWithValidation(flightWhereToAssignSeats, serviceLibrarySeatDTOToBeAssigned);


            //ASSERT
            Assert.IsTrue(actual);
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Impl.ServiceLibrary")]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AssignSeatWithValidation_Throw_Exception_If_FlighNumber_Is_Empty()
        {
            //ARRANGE
            FlightDTO flightWhereToAssignSeats = new FlightDTO(string.Empty, DateTime.Now.AddHours(3));
            SeatDTO serviceLibrarySeatDTOToBeAssigned = new SeatDTO("A", "2");

            //ACT
            Sut.AssignSeatWithValidation(flightWhereToAssignSeats, serviceLibrarySeatDTOToBeAssigned);

            //ASSERT

        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Impl.ServiceLibrary")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_AssignSeatWithValidation_Throw_Exception_If_SeatsDTO_is_Default()
        {
            //ARRANGE
            FlightDTO flightWhereToAssignSeats = new FlightDTO("FN1", DateTime.Now.AddHours(3));
            SeatDTO serviceLibrarySeatDTOToBeAssigned = default(SeatDTO);

            //ACT
            Sut.AssignSeatWithValidation(flightWhereToAssignSeats, serviceLibrarySeatDTOToBeAssigned);


            //ASSERT

        }

    }

}
