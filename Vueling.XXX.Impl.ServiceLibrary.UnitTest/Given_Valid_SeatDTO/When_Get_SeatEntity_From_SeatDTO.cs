using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain;
using Vueling.XXX.Impl.ServiceLibrary.UnitTest.Helpers;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary.UnitTest.Given_Valid_SeatDTO
{

    [TestClass]
    public class When_Get_SeatEntity_From_SeatDTO
    {

        #region .: given scenario :.

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {



        }

        #endregion

        [TestMethod()]
        [TestCategory("UnitTest"), TestCategory("Vueling.XXX.Impl.ServiceLibrary")]
        public void Then_SeatDTOToSeatEntityFactory_Maps_Correctly_If_SeatDTO_Is_Valid()
        {
            //ARRANGE
            string row = "2";
            string colum = "B";
            SeatDTO seatDTO = new SeatDTO(row, colum);
            Seat expectedSeat = SeatEntityScenarioFactory.GetvalidSeatMockedScenario(row, colum);

            //ACTION
            var seatDTOToSeatEntityFactory = new SeatDTOToSeatEntityFactory();
            Seat actualSeat = seatDTOToSeatEntityFactory.Get<SeatDTO, Seat>(seatDTO);

            //ASSERT
            SeatEntityComparerAllFileds.AssertAreEqual(expectedSeat, actualSeat);

        }

    }


}
