using System.Collections.Generic;
using System.ServiceModel;
using Vueling.XXX.WCF.WebService.DTO;
namespace Vueling.XXX.WCF.WebService
{
    [ServiceContract]
    public interface IBookingService
    {
        [OperationContract]
        string CreateBooking(int amount);

        [OperationContract]
        List<BookingResponse> GetBookings(int page, int pageSize);

        [OperationContract]
        List<BookingResponse> GetCanceledBookings(int page, int pageSize);

        [OperationContract]
        int InfuriateClients();

        [OperationContract]
        int MakeManyFriends();

    }
}
