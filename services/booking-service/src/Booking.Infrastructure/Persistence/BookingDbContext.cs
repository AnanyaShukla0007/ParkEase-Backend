using BookingEntity = Booking.Domain.Entities.Booking;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options)
        : base(options)
    {
    }

    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingEntity>(entity =>
        {
            entity.ToTable("Bookings");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.VehiclePlate)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Notes)
                .HasMaxLength(500);

            entity.Property(x => x.EstimatedAmount)
                .HasColumnType("numeric(18,2)");

            entity.Property(x => x.FinalAmount)
                .HasColumnType("numeric(18,2)");
        });
    }
}