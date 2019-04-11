using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;
using Vueling.XXX.Library.DomainServicesImplementations;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.Library.InfrastructureContracts;

namespace Vueling.XXX.Library.UnitTest.Given_AircraftWithAvailableSeats
{

    [TestClass]
    public class When_AircraftRecoveryAndPersist
    {

        #region .: given scenario :.

        static AircraftRecoverAndPersist Sut;
        static Mock<IAircraftRepository> _IAircraftRepositoryMocked;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //INITIALIZED ALL THE MOCK OBJECTS NEEDED FOR TESTING
            MockFactory mockFactory = new MockFactory();

            _IAircraftRepositoryMocked = mockFactory.CreateMock<IAircraftRepository>();

            //INITIALIZED SERVICE TO BE TESTED
            Sut = new AircraftRecoverAndPersist(_IAircraftRepositoryMocked.MockObject);
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Library")]
        [ExpectedException(typeof(AircraftParamIsNullException))]
        public void Then_ValidateAircraftContent_Throw_AircraftParamIsNullException_If_Aircraft_Is_Null()
        {
            //ARRANGE
            string flightNumber = "FN1";
            DateTime departureDate = DateTime.Now;

            Aircraft aircraft = null;

            //ACT
            Sut.ValidateAircraftContent(aircraft, flightNumber, departureDate);

            //ASSERT

        }


    }


}
