using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.DB.Infrastructure.UnitTest.Helpers;
using Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.AircraftRepository;

[assembly: CLSCompliant(false)]
namespace Vueling.XXX.DB.Infrastructure.UnitTest.Given_AircraftWithAvailableSeats
{
    [TestClass]
    public class When_Get_Aircraft_Entity_From_DB
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
        [Description("Recover an entity from a database object and validate that available seats are correct")]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.DB.Infrastructure")]
        public void Then_Aircraft_Entity_Is_Mapped_Correctly_If_AIrcraft_From_DB_Is_Valid()
        {
            //ARRANGE
            AircraftDbObjectToAircraftEntityFactory AircraftEntityFactory = new AircraftDbObjectToAircraftEntityFactory();
            var expected = AircraftScenarioFactory.GetMockedEntityAircraftWithAvailableSeats(_flightnumber, _testDate);
            var dbobject = AircraftScenarioFactory.GetMockedDbObjectAircraftWithAvailableSeats(_flightnumber, _testDate);

            //ACTION
            Library.Entities.Aircraft actual = AircraftEntityFactory.GetEntityFromDbObject<Infrastructure.Repositories.AircraftRepository.Aircraft, Library.Entities.Aircraft>(dbobject);

            //ASSERT
            AircraftEntityComparerAllFields.AssertAreEqual(expected, actual);
        }


    }
}
