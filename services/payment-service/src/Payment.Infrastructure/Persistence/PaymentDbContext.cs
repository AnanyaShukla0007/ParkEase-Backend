using Microsoft.EntityFrameworkCore;
using PaymentEntity = Payment.Domain.Entities.Payment;

namespace Payment.Infrastructure.Persistence;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
        : base(options)
    {
    }

    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentEntity>(entity =>
        {
            entity.ToTable("Payments");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Currency)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(x => x.TransactionReference)
                .HasMaxLength(100);

            entity.Property(x => x.ProviderReference)
                .HasMaxLength(100);

            entity.Property(x => x.Notes)
                .HasMaxLength(500);

            entity.Property(x => x.Amount)
                .HasColumnType("numeric(18,2)");

            entity.HasIndex(x => x.BookingId);
            entity.HasIndex(x => x.UserId);
        });
    }
}
