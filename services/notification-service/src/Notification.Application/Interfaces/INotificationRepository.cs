using Notification.Domain.Enums;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Application.Interfaces;

public interface INotificationRepository
{
    Task<NotificationEntity> AddAsync(NotificationEntity notification);
    Task AddRangeAsync(List<NotificationEntity> notifications);
    Task<NotificationEntity?> FindByNotificationIdAsync(int notificationId);
    Task<List<NotificationEntity>> FindByRecipientIdAsync(int recipientId);
    Task<List<NotificationEntity>> FindByRecipientIdAndIsReadAsync(int recipientId, bool isRead);
    Task<int> CountByRecipientIdAndIsReadAsync(int recipientId, bool isRead);
    Task<List<NotificationEntity>> FindByTypeAsync(NotificationType type);
    Task<List<NotificationEntity>> FindByRelatedIdAsync(int relatedId);
    Task<List<NotificationEntity>> GetAllAsync();
    Task UpdateAsync(NotificationEntity notification);
    Task DeleteByNotificationIdAsync(NotificationEntity notification);
}
