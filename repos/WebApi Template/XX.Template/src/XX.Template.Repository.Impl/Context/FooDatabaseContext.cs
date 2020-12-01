using XX.Template.Repository.Impl.Entity;
using Microsoft.EntityFrameworkCore;

namespace XX.Template.Repository.Impl.Context
{
    public class FooDatabaseContext : DbContext
    {
        public FooDatabaseContext(DbContextOptions<FooDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingLogEntity> BookingLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingLogEntity>(entity =>
            {
                entity.ToTable("BookingLog");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.RecordLocator)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}