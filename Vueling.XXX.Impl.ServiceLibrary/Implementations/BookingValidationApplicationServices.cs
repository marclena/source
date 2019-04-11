using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterServiceAttribute]
    public class BookingValidationApplicationServices : Vueling.XXX.Contracts.ServiceLibrary.IBookingValidationApplicationServices
    {
        private readonly Vueling.XXX.Library.DomainServicesContracts.IBookingFeaturesDomainServices _BookingValidationDomainServices;

        public BookingValidationApplicationServices(Vueling.XXX.Library.DomainServicesContracts.IBookingFeaturesDomainServices _IBookingValidationDomainServices)
        {
            _BookingValidationDomainServices = _IBookingValidationDomainServices;
        }

        public bool IsAgency(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return _BookingValidationDomainServices.IsAgency(booking);
        }

        public bool IsCorporate(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return _BookingValidationDomainServices.IsCorporate(booking);
        }

        public bool IsEnabledToAddNewJourneys(BookingDTO bookingDto)
        {
            var booking = GetMapped(bookingDto);

            return _BookingValidationDomainServices.IsEnabledToAddNewJourneys(booking);
        }

        private Vueling.XXX.Library.Entities.Booking GetMapped(BookingDTO bookingDto)
        {
            var bookingMapper = MappingToDomainFactory.GetFor(EnumDomain.Booking);
            return bookingMapper.Get<BookingDTO, Vueling.XXX.Library.Entities.Booking>(bookingDto);
        }
    }
}
