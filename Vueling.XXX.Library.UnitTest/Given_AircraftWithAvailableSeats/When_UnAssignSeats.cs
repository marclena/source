using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Library.DomainServicesImplementations;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.UnitTest.Helpers;
using Vueling.XXX.Library.Configuration;

[assembly: CLSCompliant(true)]
namespace Vueling.XXX.Library.UnitTest.Given_AircraftWithAvailableSeats
{
    [TestClass]
    public class When_UnAssignSeats
    {

        #region .: given scenario :.

        static string flightNumber = "FN1";
        static DateTime departureDate = DateTime.Now;
        static Seat seatToUnAssign = new Seat
        {
            Availability = AvailabilityEnum.Busy,
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

            //INITIALIZED OBJECT CONTROLLER FOR UNIT TESTING
            Sut = new SeatAssignment(_IAircraftRecoverAndPersistMocked.MockObject, _libraryConfigurationMocked.MockObject);

            //COMPLETE SCENARIO
            _libraryConfigurationMocked.Expects.One.GetProperty(v => v.TimeSalesCloseBeforeFlight).WillReturn(2);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.UpdateAircraft(default(Aircraft))).WithAnyArguments().WillReturn(true);
        }

        #endregion


        [TestMethod]
        [Description("when all input parameters are ok unassignment complete successfuly.")]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Library")]
        public void And_ParametersOK_Then_AssigSeat_ReturnTrue()
        {
            //ARRANGE
            var aircraft = AircraftScenarioFactory.GetMockedAircraftWithAvailableSeats(flightNumber, departureDate);
            _IAircraftRecoverAndPersistMocked.Expects.One.Method(x => x.GetAircraftFromRepository(aircraft)).WithAnyArguments().WillReturn(aircraft);

            //ACTION
            bool actual = Sut.Unassign(aircraft, seatToUnAssign);

            //ASSERT

            Assert.IsTrue(actual, "No se esparaba error al desasignar un asiento cuando los parametros de entrada son correctos.");
        }

    }
}
