using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Payment.Infrastructure.Persistence;

public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
{
    public PaymentDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=parkease_payment_db;Username=postgres;Password=AnaPassword");

        return new PaymentDbContext(optionsBuilder.Options);
    }
}
