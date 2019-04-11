using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Library.Configuration;
using Vueling.XXX.Library.DomainServicesImplementations;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.UnitTest.Helpers;

namespace Vueling.XXX.Library.UnitTest.Given_AircraftWithAvailableSeats
{
    [TestClass]
    public class When_AssignSeats
    {

        #region .: given scenario :.

        static string flightNumber = "FN1";
        static DateTime departureDate = DateTime.Now;
        static Seat seatToAssign = new Seat
        {
            Availability = AvailabilityEnum.Available,
            Column = "B",
            Row = "2"
        };

        static SeatAssignment Sut;
        static Mock<IAircraftRecoverAndPersist> _IAircraftRecoverAndPersistMocked;
        static Mock<IXXXLibraryConfiguration> _libraryConfigurationMocked;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            MockFactory mockFactory = new MockFactory();

            _libraryConfigurationMocked = mockFactory.CreateMock<IXXXLibraryConfiguration>();
            _IAircraftRecoverAndPersistMocked = mockFactory.CreateMock<IAircraftRecoverAndPersist>();

            //INITIALIZED SERVICE TO BE TESTED
            Sut = new SeatAssignment(_IAircraftRecoverAndPersistMocked.MockObject, _libraryConfigurationMocked.MockObject);

            //COMPLETE SCENARIO
            _libraryConfigurationMocked.Expects.One.GetProperty(v => v.TimeSalesCloseBeforeFlight).WillReturn(2);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.UpdateAircraft(default(Aircraft))).WithAnyArguments().WillReturn(true);
        }

        #endregion


        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Library")]
        public void Then_AssigSeat_Returns_True_If_Parameters_Are_OK()
        {
            //ARRANGE
            seatToAssign.Column = "B";

            var aircraft = AircraftScenarioFactory.GetMockedAircraftWithAvailableSeats(flightNumber, departureDate);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.GetAircraftFromRepository(aircraft)).WithAnyArguments().WillReturn(aircraft);

            //ACT
            bool actual = Sut.Assign(aircraft, seatToAssign);

            //ASSERT
            Assert.IsTrue(actual, "Se esparaba una asignación de asientos sin errores cuando los parametros de entrada son correctos.");
        }

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Library")]
        public void Then_AssigSeat_Returns_False_If_Parameters_Are_Not_OK()
        {
            //ARRANGE
            seatToAssign.Column = "";

            var aircraft = AircraftScenarioFactory.GetMockedAircraftWithAvailableSeats(flightNumber, departureDate);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.GetAircraftFromRepository(aircraft)).WithAnyArguments().WillReturn(aircraft);

            //ACT
            bool actual = Sut.Assign(aircraft, seatToAssign);

            //ASSERT
            Assert.IsFalse(actual, "Se esparaba un error en la asignación de asientos cuando los parametros de entrada no son correctos.");
        }

    }
    
}
