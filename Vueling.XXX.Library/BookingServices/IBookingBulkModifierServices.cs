using System.Collections.Generic;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.BookingServices
{
    public interface IBookingBulkModifierServices
    {
        void DivideJourneysPrices(List<Booking> bookings);

        void ModifyAllItemsInBookings(IEnumerable<Booking> bookings);
    }
}
