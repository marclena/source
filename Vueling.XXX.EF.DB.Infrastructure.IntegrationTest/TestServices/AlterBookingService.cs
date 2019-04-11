using System;
using System.Linq;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.TestServices
{
    internal class AlterBookingService
    {
        internal static void ApplyInsertDeleteAndModifyActions(Booking firstBooking)
        {
            firstBooking.RecordLocator = Guid.NewGuid().ToString();

            DeleteCreateJourneys(firstBooking);

            ModifyCreatePassengers(firstBooking);
        }

        private static void DeleteCreateJourneys(Booking firstBooking)
        {
            if (firstBooking.Journeys.Any()) { firstBooking.Journeys.Clear(); }

            //if (!_BookingValidationDomainServices.IsEnabledToAddNewJourneys(firstBooking)) { return; }

            var journey = new Journey
            {
                Arrival = "AGP",
                ArrivalDate = DateTime.Now.AddDays(10),
                BookingId = firstBooking.Id,
                Departure = "MAD",
                Price = 99.99M,
                DepartureDate = DateTime.Now.AddDays(9)
            };

            firstBooking.AddJourney(journey);
        }

        private static void ModifyCreatePassengers(Booking firstBooking)
        {
            if (firstBooking.Passengers != null)
            {
                var firstPax = firstBooking.Passengers.FirstOrDefault();
                firstPax.FullName = string.Format("Modified Pax {0}", DateTime.Now);
            }

            var passenger = new Passenger
            {
                BookingId = firstBooking.Id,
                FullName = DateTime.Now.ToString(),
                PaxType = Passenger.PassengerType.ADU
            };

            firstBooking.AddPassenger(passenger);
        }
    }
}
