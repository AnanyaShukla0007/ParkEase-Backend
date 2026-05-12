using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Booking.Infrastructure.Persistence;

public class BookingDbContextFactory
    : IDesignTimeDbContextFactory<BookingDbContext>
{
    public BookingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=parkease_booking_db;Username=postgres;Password=AnaPassword");

        return new BookingDbContext(optionsBuilder.Options);
    }
}