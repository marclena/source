using System.Linq;
using Vueling.XXX.Library.Entities;
namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface IBookingDomainServices
    {
        //IQueryable<Booking> Get(
        //    Expression<Func<Booking, bool>> filter = null,
        //    Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null,
        //    List<Expression<Func<Booking, object>>> includeProperties = null,
        //    int? page = null, int? pageSize = null, bool trackingEnabled = false);
        IQueryable<Booking> GetAll();

        int CreateSampleBooking(int amount);

        IQueryable<Booking> GetActives();

        IQueryable<Booking> GetCanceled();

        int ChangeFlights();
        
        int DividePrices();

    }
}
