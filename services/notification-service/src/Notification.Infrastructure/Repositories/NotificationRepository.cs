using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Enums;
using Notification.Infrastructure.Persistence;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationEntity> AddAsync(NotificationEntity notification)
    {
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task AddRangeAsync(List<NotificationEntity> notifications)
    {
        await _context.Notifications.AddRangeAsync(notifications);
        await _context.SaveChangesAsync();
    }

    public async Task<NotificationEntity?> FindByNotificationIdAsync(int notificationId)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(x => x.NotificationId == notificationId);
    }

    public async Task<List<NotificationEntity>> FindByRecipientIdAsync(int recipientId)
    {
        return await _context.Notifications
            .Where(x => x.RecipientId == recipientId)
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();
    }

    public async Task<List<NotificationEntity>> FindByRecipientIdAndIsReadAsync(int recipientId, bool isRead)
    {
        return await _context.Notifications
            .Where(x => x.RecipientId == recipientId && x.IsRead == isRead)
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();
    }

    public Task<int> CountByRecipientIdAndIsReadAsync(int recipientId, bool isRead)
    {
        return _context.Notifications.CountAsync(x => x.RecipientId == recipientId && x.IsRead == isRead);
    }

    public async Task<List<NotificationEntity>> FindByTypeAsync(NotificationType type)
    {
        return await _context.Notifications
            .Where(x => x.Type == type)
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();
    }

    public async Task<List<NotificationEntity>> FindByRelatedIdAsync(int relatedId)
    {
        return await _context.Notifications
            .Where(x => x.RelatedId == relatedId)
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();
    }

    public async Task<List<NotificationEntity>> GetAllAsync()
    {
        return await _context.Notifications
            .OrderByDescending(x => x.SentAt)
            .ToListAsync();
    }

    public async Task UpdateAsync(NotificationEntity notification)
    {
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByNotificationIdAsync(NotificationEntity notification)
    {
        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
    }
}
