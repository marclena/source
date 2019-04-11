using System;
using System.Collections.Generic;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.DB.Infrastructure.UnitTest.Helpers
{
    class AircraftScenarioFactory
    {

        private AircraftScenarioFactory()
        {

        }

        internal static Aircraft GetMockedEntityAircraftWithAvailableSeats(string flightNumber, DateTime testDate)
        {
            return new Aircraft
                (
                    flightNumber, testDate,
                    new List<Seat>
                {
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Busy, Column = "A", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "B", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "C", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "D", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Available, Column = "E", Row = "1" },
                    new Seat { Availability = Library.Entities.AvailabilityEnum.Busy, Column = "F", Row = "1" }
                }
                );
        }

        internal static Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository.Aircraft GetMockedDbObjectAircraftWithAvailableSeats(string flightNumber, DateTime testDate)
        {
            return new Infrastructure.Repositories.AircraftRepository.Aircraft
            {
                FlightNumber = flightNumber,
                DepartureDate = testDate,
                BusySeats = "1A,1F",
                Seats = "1A,1B,1C,1D,1E,1F"
            };
        }

    }
}
