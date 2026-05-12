using Microsoft.EntityFrameworkCore;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Infrastructure.Persistence;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.ToTable("Notifications");

            entity.HasKey(x => x.NotificationId);

            entity.Property(x => x.NotificationId)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.RelatedType)
                .HasMaxLength(100);

            entity.HasIndex(x => x.RecipientId);
            entity.HasIndex(x => new { x.RecipientId, x.IsRead });
            entity.HasIndex(x => x.Type);
            entity.HasIndex(x => x.RelatedId);
        });
    }
}
