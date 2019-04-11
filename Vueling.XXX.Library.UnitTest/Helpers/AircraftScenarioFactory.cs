using System;
using System.Collections.Generic;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.UnitTest.Helpers
{
    internal class AircraftScenarioFactory
    {

        private AircraftScenarioFactory()
        {

        }

        internal static Entities.Aircraft GetMockedAircraftWithAvailableSeats(string flightNumber, DateTime departureDate)
        {
            List<Seat> seats = new List<Entities.Seat>
                {
                    new Seat { Row = "1", Column = "A", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "1", Column = "B", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "1", Column = "C", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "1", Column = "D", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "1", Column = "E", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "1", Column = "F", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "2", Column = "A", Availability = AvailabilityEnum.Busy },
                    new Seat { Row = "2", Column = "B", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "2", Column = "C", Availability = AvailabilityEnum.Busy },
                    new Seat { Row = "2", Column = "D", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "2", Column = "E", Availability = AvailabilityEnum.Busy },
                    new Seat { Row = "2", Column = "F", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "A", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "B", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "C", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "D", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "E", Availability = AvailabilityEnum.Available },
                    new Seat { Row = "3", Column = "F", Availability = AvailabilityEnum.Available }
                };

            return new Entities.Aircraft(flightNumber, departureDate, seats);
        }
    }
}
