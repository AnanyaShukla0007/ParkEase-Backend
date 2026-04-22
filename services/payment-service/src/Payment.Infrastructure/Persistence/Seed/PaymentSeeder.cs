using Payment.Domain.Enums;
using PaymentEntity = Payment.Domain.Entities.Payment;

namespace Payment.Infrastructure.Persistence.Seed;

public static class PaymentSeeder
{
    public static async Task SeedAsync(PaymentDbContext context)
    {
        if (context.Payments.Any())
            return;

        var items = new List<PaymentEntity>
        {
            new()
            {
                BookingId = 1,
                UserId = 1,
                Amount = 80,
                Currency = "INR",
                PaymentMethod = PaymentMethod.Card,
                Status = PaymentStatus.Pending,
                Notes = "Seed pending payment"
            },
            new()
            {
                BookingId = 2,
                UserId = 2,
                Amount = 120,
                Currency = "INR",
                PaymentMethod = PaymentMethod.Upi,
                Status = PaymentStatus.Paid,
                TransactionReference = "PAY-SEED-1001",
                ProviderReference = "UPI-SEED-1001",
                PaidAtUtc = DateTime.UtcNow.AddDays(-1),
                Notes = "Seed paid payment"
            }
        };

        await context.Payments.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}
