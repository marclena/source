using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.DB.Infrastructure.UnitTest.Helpers;
using Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.AircraftRepository;

namespace Vueling.XXX.DB.Infrastructure.UnitTest.Given_AircraftWithAvailableSeats
{

    [TestClass]
    public class When_Get_DBObject_Aircraft_From_Aircraft_Entity
    {

        #region .: given scenario :.

        private static string _flightnumber;
        private static DateTime _testDate;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _flightnumber = "FN100";
            _testDate = DateTime.Now.AddDays(2);
        }

        #endregion

        [TestMethod()]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Infrastructure")]
        public void Then_Aircraft_From_DB_Is_Mapped_Correctly_If_AIrcraft_Entity_Is_Valid()
        {

            //ARRANGE
            AircraftEntityToAircraftDbObjectFactory AircraftEntityFactory = new AircraftEntityToAircraftDbObjectFactory();
            Infrastructure.Repositories.AircraftRepository.Aircraft expected = AircraftScenarioFactory.GetMockedDbObjectAircraftWithAvailableSeats(_flightnumber, _testDate);
            Vueling.XXX.Library.Entities.Aircraft aircraftEntity = AircraftScenarioFactory.GetMockedEntityAircraftWithAvailableSeats(_flightnumber, _testDate);

            //ACTION
            Infrastructure.Repositories.AircraftRepository.Aircraft actual = AircraftEntityFactory.GetDbObjectFromEntity<Vueling.XXX.Library.Entities.Aircraft, Infrastructure.Repositories.AircraftRepository.Aircraft>(aircraftEntity);

            //ASSERT
            AircraftDBObjectComparerAllFileds.AssertAreEqual(expected, actual);
        }

    }

}
