using Notification.Application.DTOs;

namespace Notification.Application.Interfaces;

public interface INotificationRealtimeDispatcher
{
    Task SendToRecipientAsync(NotificationResponse notification);
}
