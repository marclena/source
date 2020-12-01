using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterServiceAttribute]
    public class BookingBusinessApplicationServices : Vueling.XXX.Contracts.ServiceLibrary.IBookingBusinessApplicationServices
    {

        public bool IsAlreadyFlew(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return booking.IsAlreadyFlew();
        }

        public decimal GetTotalPrice(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return booking.GetTotalPrice();
        }

        public string GetRoute(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return booking.GetRoute();
        }

        private Vueling.XXX.Library.Entities.Booking GetMapped(BookingDTO bookingDto)
        {
            var bookingMapper = MappingToDomainFactory.GetFor(EnumDomain.Booking);
            return bookingMapper.Get<BookingDTO, Vueling.XXX.Library.Entities.Booking>(bookingDto);
        }
    }
}
