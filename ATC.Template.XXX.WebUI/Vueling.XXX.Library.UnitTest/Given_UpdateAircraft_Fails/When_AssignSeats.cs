using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Library.Configuration;
using Vueling.XXX.Library.DomainServicesImplementations;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.UnitTest.Helpers;

namespace Vueling.XXX.Library.UnitTest.Given_UpdateAircraft_Fails
{
    [TestClass]
    public class When_AssignSeats
    {

        #region .: given scenario :.

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
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.UpdateAircraft(default(Aircraft))).WithAnyArguments().WillReturn(false);
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Library")]
        [ExpectedException(typeof(UpdateToRepositoryException))]
        public void Then_AssignSeat_Thrown_Exception_If_TimeSalesCloseBeforeFlight()
        {
            //ARRANGE
            string flightNumber = "FN1";
            DateTime departureDate = DateTime.Now;
            Seat seatToAssign = new Seat
            {
                Availability = AvailabilityEnum.Available,
                Column = "B",
                Row = "2"
            };

            var aircraft = AircraftScenarioFactory.GetMockedAircraftWithAvailableSeats(flightNumber, departureDate);

            _libraryConfigurationMocked.Expects.One.GetProperty(v => v.TimeSalesCloseBeforeFlight).WillReturn(2);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.GetAircraftFromRepository(aircraft)).WithAnyArguments().WillReturn(aircraft);

            //ACT
            Sut.Assign(aircraft, seatToAssign);

            //ASSERT

        }

    }
    
}
