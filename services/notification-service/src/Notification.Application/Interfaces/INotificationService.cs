using Notification.Application.DTOs;

namespace Notification.Application.Interfaces;

public interface INotificationService
{
    Task<NotificationResponse> SendAsync(SendNotificationRequest request);
    Task<List<NotificationResponse>> SendBulkAsync(SendBulkNotificationsRequest request);
    Task<NotificationResponse> MarkAsReadAsync(int notificationId);
    Task<int> MarkAllReadAsync(int recipientId);
    Task<List<NotificationResponse>> GetByRecipientAsync(int recipientId);
    Task<List<NotificationResponse>> GetUnreadByRecipientAsync(int recipientId);
    Task<int> GetUnreadCountAsync(int recipientId);
    Task DeleteNotificationAsync(int notificationId);
    Task SendEmailAsync(NotificationResponse notification);
    Task SendSmsAsync(NotificationResponse notification);
    Task<List<NotificationResponse>> GetAllAsync();
}
