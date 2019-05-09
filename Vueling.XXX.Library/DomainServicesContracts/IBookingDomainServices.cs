using System.Linq;
using Vueling.XXX.Library.Entities;
namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface IBookingDomainServices
    {
  
        IQueryable<Booking> GetAll();

        int CreateSampleBooking(int amount);

        IQueryable<Booking> GetActives();

        IQueryable<Booking> GetCanceled();

        int ChangeFlights();
        
        int DividePrices();

    }
}
