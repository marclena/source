using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterServiceAttribute]
    public class BookingCancelerApplicationServices : Vueling.XXX.Contracts.ServiceLibrary.IBookingCancelerApplicationServices
    {
        private readonly Vueling.XXX.Library.DomainServicesContracts.IBookingCancelerDomainServices _BookingCancelerDomainServices;
        public BookingCancelerApplicationServices(Vueling.XXX.Library.DomainServicesContracts.IBookingCancelerDomainServices _IBookingCancelerDomainServices)
        {
            _BookingCancelerDomainServices = _IBookingCancelerDomainServices;
        }

        public int CancelBooking(int bookingId)
        {
            return _BookingCancelerDomainServices.CancelBooking(bookingId);
        }
    }
}
