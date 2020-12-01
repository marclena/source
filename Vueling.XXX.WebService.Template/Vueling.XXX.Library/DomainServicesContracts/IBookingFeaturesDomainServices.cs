namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface IBookingFeaturesDomainServices
    {
        bool IsAgency(Vueling.XXX.Library.Entities.Booking booking);
        bool IsCorporate(Vueling.XXX.Library.Entities.Booking booking);
        bool IsEnabledToAddNewJourneys(Vueling.XXX.Library.Entities.Booking booking);
    }
}
