namespace Vueling.XXX.Contracts.ServiceLibrary
{
    public interface IBookingValidationApplicationServices
    {
        bool IsAgency(Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto);
        bool IsCorporate(Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto);
        bool IsEnabledToAddNewJourneys(Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto);
    }
}
