using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary.UnitTest.Helpers
{
    internal class SeatEntityScenarioFactory
    {

        private SeatEntityScenarioFactory()
        {

        }

        internal static Seat GetvalidSeatMockedScenario(string row, string colum)
        {

            Seat seatReturned = new Seat
            {

                Row = row,
                Column = colum,
                Availability = Vueling.XXX.Library.Entities.AvailabilityEnum.Available

            };

            return seatReturned;
        
        }

    }
}
