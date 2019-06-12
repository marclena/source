using log4net;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary;

namespace Vueling.XXX.WCF.WebService
{
    [RegisterServiceAttribute]
    public class BookingCancelerSecureService : IBookingCancelerSecureService
    {
        private readonly IBookingCancelerApplicationServices _BookingCancelerApplicationServices;

        public BookingCancelerSecureService(IBookingCancelerApplicationServices _IBookingCancelerApplicationServices)
        {
            _BookingCancelerApplicationServices = _IBookingCancelerApplicationServices;
        }

        public int CancelBooking(int bookingId)
        {
            return _BookingCancelerApplicationServices.CancelBooking(bookingId);
        }
    }
}
