using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Contracts.ServiceLibrary
{
    public interface IBookingBusinessApplicationServices
    {
        decimal GetTotalPrice(Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto);
        bool IsAlreadyFlew(Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto);
        string GetRoute(BookingDTO bookingDto);
    }
}
