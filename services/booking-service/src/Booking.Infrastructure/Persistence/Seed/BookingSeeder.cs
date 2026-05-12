using Booking.Domain.Enums;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Infrastructure.Persistence.Seed;

public static class BookingSeeder
{
    public static async Task SeedAsync(BookingDbContext context)
    {
        if (context.Bookings.Any())
            return;

        var items = new List<BookingEntity>
        {
            new BookingEntity
            {
                UserId = 1,
                LotId = 1,
                SpotId = 1,
                VehicleId = 1,
                VehiclePlate = "UP32AB1234",
                BookingType = BookingType.PreBooking,
                Status = BookingStatus.Confirmed,
                PaymentState = PaymentState.NotRequired,
                StartTimeUtc = DateTime.UtcNow.AddHours(1),
                EndTimeUtc = DateTime.UtcNow.AddHours(3),
                EstimatedAmount = 80,
                FinalAmount = 0,
                Notes = "Seed booking"
            },
            new BookingEntity
            {
                UserId = 2,
                LotId = 1,
                SpotId = 2,
                VehicleId = 2,
                VehiclePlate = "DL01CD5678",
                BookingType = BookingType.WalkIn,
                Status = BookingStatus.Active,
                PaymentState = PaymentState.Pending,
                StartTimeUtc = DateTime.UtcNow.AddHours(-1),
                EndTimeUtc = DateTime.UtcNow.AddHours(2),
                CheckInTimeUtc = DateTime.UtcNow.AddMinutes(-45),
                EstimatedAmount = 120,
                FinalAmount = 0,
                Notes = "Active seed booking"
            }
        };

        await context.Bookings.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}