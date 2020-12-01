using log4net;
using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.WCF.WebService
{
    [RegisterServiceAttribute]
    public class BookingCancelerSecureService : IBookingCancelerSecureService
    {
        static ILog Logger = LogManager.GetLogger(typeof(BookingCancelerSecureService));

        private readonly Vueling.XXX.Contracts.ServiceLibrary.IBookingCancelerApplicationServices _BookingCancelerApplicationServices;

        public BookingCancelerSecureService(Vueling.XXX.Contracts.ServiceLibrary.IBookingCancelerApplicationServices _IBookingCancelerApplicationServices)
        {
            _BookingCancelerApplicationServices = _IBookingCancelerApplicationServices;
        }

        public int CancelBooking(int bookingId)
        {
            return _BookingCancelerApplicationServices.CancelBooking(bookingId);
        }
    }
}
