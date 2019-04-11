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

        public BookingContext(IXXXInfrastructureConfiguration _IXXXInfrastructureConfiguration)
            : base(_IXXXInfrastructureConfiguration.DatabaseConnectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //EF can delete orphans automatically only if you use the identifying relationship. 
            //You need to define a composite key for the Journey object, that consists of Id and BookingId.

            //modelBuilder.Entity<Journey>()
            //.HasKey(o => new { o.Id, o.BookingId });

            //modelBuilder.Entity<Journey>()
            //            .Property(o => o.Id)
            //            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            //modelBuilder.Entity<Passenger>()
            //.HasKey(o => new { o.Id, o.BookingId });

            //modelBuilder.Entity<Passenger>()
            //            .Property(o => o.Id)
            //            .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Vueling.XXX.Library.Entities.Booking> Bookings { get; set; }
        public virtual DbSet<Vueling.XXX.Library.Entities.Journey> Journeys { get; set; }
        public virtual DbSet<Vueling.XXX.Library.Entities.Passenger> Passengers { get; set; }
        
    }
}
