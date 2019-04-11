using System.ServiceModel;
namespace Vueling.XXX.WCF.WebService
{
    [ServiceContract]
    public interface IBookingCancelerSecureService
    {
        [OperationContract]
        int CancelBooking(int bookingId);

    }
}
