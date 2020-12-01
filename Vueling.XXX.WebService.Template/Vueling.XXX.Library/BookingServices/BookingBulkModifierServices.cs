using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.BookingServices
{
    [RegisterServiceAttribute]
    public class BookingBulkModifierServices : Vueling.XXX.Library.BookingServices.IBookingBulkModifierServices
    {
        private readonly IBookingFeaturesDomainServices _BookingValidationDomainServices;

        public BookingBulkModifierServices(IBookingFeaturesDomainServices _IBookingValidationDomainServices)
        {
            _BookingValidationDomainServices = _IBookingValidationDomainServices;
        }

        public void DivideJourneysPrices(List<Booking> bookings)
        {
            bookings.ForEach(booking => DividePriceForJourneysInSingleBooking(booking));
        }

        private void DividePriceForJourneysInSingleBooking(Booking booking)
        {
            if (!booking.Journeys.Any() || booking.IsAlreadyFlew()) { return; }

            booking.Modified = DateTime.UtcNow;

            foreach (var journey in booking.Journeys) { journey.Price = journey.Price / 2; }
        }

        public void ModifyAllItemsInBookings(IEnumerable<Booking> bookings)
        {
            foreach (var booking in bookings)
            {
                ModifyAllItemsInSingleBooking(booking);
            }
        }

        private void ModifyAllItemsInSingleBooking(Booking firstBooking)
        {
            firstBooking.RecordLocator = Guid.NewGuid().ToString();

            DeleteCreateJourneys(firstBooking);

            ModifyCreatePassengers(firstBooking);
        }

        private void DeleteCreateJourneys(Booking firstBooking)
        {
            if (firstBooking.Journeys.Any()) { firstBooking.Journeys.Clear(); }

            if (!_BookingValidationDomainServices.IsEnabledToAddNewJourneys(firstBooking)) { return; }

            var journey = new Journey
            {
                Arrival = "AGP",
                ArrivalDate = DateTime.UtcNow.AddDays(10),
                BookingId = firstBooking.Id,
                Departure = "MAD",
                Price = 99.99M,
                DepartureDate = DateTime.UtcNow.AddDays(9)
            };

            firstBooking.AddJourney(journey);
        }

        private void ModifyCreatePassengers(Booking firstBooking)
        {
            if (firstBooking.Passengers != null)
            {
                var firstPax = firstBooking.Passengers.FirstOrDefault();
                firstPax.FullName = string.Format("Modified Pax {0}", DateTime.UtcNow);
            }

            var passenger = new Passenger
            {
                BookingId = firstBooking.Id,
                FullName = DateTime.UtcNow.ToString(),
                PaxType = Passenger.PassengerType.ADU
            };

            firstBooking.AddPassenger(passenger);
        }

    }
}
