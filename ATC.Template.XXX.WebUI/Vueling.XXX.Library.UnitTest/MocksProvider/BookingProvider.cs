using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.UnitTest.MocksProvider
{
    internal static class BookingProvider
    {
        internal static IQueryable<Booking> GetBookings(string salesAgent, int amountOfBooking)
        {
            List<Booking> bookings = new List<Booking>();

            for (int i = 0; i < amountOfBooking; i++)
            {
                bookings.Add(GetNewBooking(salesAgent, i + 1));
            }

            return bookings.AsQueryable();
        }

        internal static Booking GetNewBooking(string salesAgent, int addDays)
        {
            return new Booking
            {
                Id = 1 + addDays,
                SalesAgent = salesAgent,
                Passengers = new List<Passenger>
                {
                    new Passenger
                    {
                        BookingId = 1 + addDays,
                        FullName = string.Format("Name{0}", 1 + addDays),
                        Id = 1 + addDays,
                        PaxType = Passenger.PassengerType.ADU
                    }
                },
                Journeys = new List<Journey>
                {
                    new Journey
                    {
                        Id = 1 + addDays,
                        Arrival = "MAD",
                        BookingId = 1 + addDays,
                        Departure = "BCN",
                        Price = 99,
                        ArrivalDate = DateTime.UtcNow.AddDays(addDays),
                        DepartureDate = DateTime.UtcNow.AddDays(addDays)
                    }
                },
                RecordLocator = "XXX000"
            };
        }
    }
}
