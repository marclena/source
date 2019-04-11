using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Configuration;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class BookingFeaturesDomainServices : Vueling.XXX.Library.DomainServicesContracts.IBookingFeaturesDomainServices
    {
        private readonly IXXXLibraryConfiguration _XXXLibraryConfiguration;

        public BookingFeaturesDomainServices(IXXXLibraryConfiguration _IXXXLibraryConfiguration)
        {
            _XXXLibraryConfiguration = _IXXXLibraryConfiguration;
        }

        public bool IsEnabledToAddNewJourneys(Booking booking)
        {
            return booking.Journeys == null || booking.Journeys.Count < _XXXLibraryConfiguration.MaxJourneysAllowedByBooking;
        }

        public bool IsAgency(Booking booking)
        {
            return !string.IsNullOrEmpty(booking.SalesAgent) && booking.SalesAgent.ToLower().Contains(_XXXLibraryConfiguration.PartialCodeForAgencyAgent);
        }

        public bool IsCorporate(Booking booking)
        {
            return !string.IsNullOrEmpty(booking.SalesAgent) && booking.SalesAgent.ToLower().Contains(_XXXLibraryConfiguration.PartialCodeForCorporateAgent);
        }
    }
}
