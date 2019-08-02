using System.Data.Entity;
using Vueling.XXX.EF.DB.Infrastructure.Configuration;

namespace Vueling.XXX.EF.DB.Infrastructure.BoundedContexts
{
    public class BookingContext : Vueling.DBAccess.EF.DB.Infrastructure.DbContextBase
    {
        static BookingContext()
        {
            Database.SetInitializer<BookingContext>(null);
        }

        public BookingContext(IInfrastructureConfiguration _IXXXInfrastructureConfiguration)
            : base(_IXXXInfrastructureConfiguration.DatabaseConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Vueling.XXX.Library.Entities.Booking> Bookings { get; set; }
        public virtual DbSet<Vueling.XXX.Library.Entities.Journey> Journeys { get; set; }
        public virtual DbSet<Vueling.XXX.Library.Entities.Passenger> Passengers { get; set; }
        
    }
}
