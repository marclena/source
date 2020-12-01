using System.Collections.Generic;
using System.Linq;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
namespace Vueling.XXX.Contracts.ServiceLibrary
{
    public interface IBookingApplicationServices
    {
        int CreateBooking(int amount);

        IQueryable<BookingDTO> GetAll();
        int GetActivesCount();
        List<BookingDTO> GetActives();
        List<BookingDTO> GetCanceled();

        List<BookingDTO> GetActivesByPages(int page, int pageSize);
        List<BookingDTO> GetCanceledByPages(int page, int pageSize);

        int ChangeFlights();
        int DividePrices();
    }
}
