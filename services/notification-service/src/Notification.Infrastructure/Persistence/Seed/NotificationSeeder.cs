using Notification.Domain.Enums;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Infrastructure.Persistence.Seed;

public static class NotificationSeeder
{
    public static async Task SeedAsync(NotificationDbContext context)
    {
        if (context.Notifications.Any())
            return;

        var items = new List<NotificationEntity>
        {
            new()
            {
                RecipientId = 1,
                Type = NotificationType.Booking,
                Title = "Booking confirmed",
                Message = "Your parking booking has been confirmed.",
                Channel = NotificationChannel.App,
                RelatedId = 1001,
                RelatedType = "Booking",
                IsRead = false
            },
            new()
            {
                RecipientId = 2,
                Type = NotificationType.Payment,
                Title = "Payment received",
                Message = "Your payment receipt is ready.",
                Channel = NotificationChannel.Email,
                RelatedId = 2001,
                RelatedType = "Payment",
                IsRead = true
            }
        };

        await context.Notifications.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}
