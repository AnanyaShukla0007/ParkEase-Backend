using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notification.Infrastructure.Persistence;

public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=parkease_notification_db;Username=postgres;Password=AnaPassword");

        return new NotificationDbContext(optionsBuilder.Options);
    }
}
