using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.AircraftRepository;

namespace Vueling.XXX.DB.Infrastructure.UnitTest.Given_AircraftWithAvailableSeats
{

    [TestClass]
    public class When_Get_DBObject_Seat_From_Seat_Entity
    {

        private static List<Seat> _seatEntityMocked;
        private static string _dbObjectSeatsMocked;

        #region .: given scenario :.

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _seatEntityMocked = new List<Seat>
                {
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Busy, Column = "A", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "B", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "C", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "D", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "E", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Busy, Column = "F", Row = "1" }
                };
            _dbObjectSeatsMocked = "1A,1B,1C,1D,1E,1F";
        }


        #endregion

        [TestMethod()]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Infrastructure")]
        public void Then_Aircraft_From_DB_Is_Mapped_Correctly_If_AIrcraft_Entity_Is_Valid()
        {

            //ARRANGE
            SeatEntityToSeatDbObjectFactory seatEntityFactory = new SeatEntityToSeatDbObjectFactory();
            string expected = _dbObjectSeatsMocked;

            //ACTION
            string actual = seatEntityFactory.GetDbObjectFromEntity<List<Seat>, string>(_seatEntityMocked);

            //ASSERT
            Assert.AreEqual(expected, actual, "Seat not mapped.");
        }

    }

}
