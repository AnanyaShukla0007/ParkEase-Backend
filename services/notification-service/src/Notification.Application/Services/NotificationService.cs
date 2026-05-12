using Notification.Application.DTOs;
using Notification.Application.Interfaces;
using Notification.Application.Validators;
using Notification.Domain.Enums;
using NotificationEntity = Notification.Domain.Entities.Notification;

namespace Notification.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationRealtimeDispatcher _realtimeDispatcher;

    public NotificationService(
        INotificationRepository notificationRepository,
        INotificationRealtimeDispatcher realtimeDispatcher)
    {
        _notificationRepository = notificationRepository;
        _realtimeDispatcher = realtimeDispatcher;
    }

    public async Task<NotificationResponse> SendAsync(SendNotificationRequest request)
    {
        var errors = SendNotificationValidator.Validate(request);

        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var notification = new NotificationEntity
        {
            RecipientId = request.RecipientId,
            Type = request.Type,
            Title = request.Title.Trim(),
            Message = request.Message.Trim(),
            Channel = request.Channel,
            RelatedId = request.RelatedId,
            RelatedType = string.IsNullOrWhiteSpace(request.RelatedType) ? null : request.RelatedType.Trim(),
            IsRead = false,
            SentAt = DateTime.UtcNow
        };

        var saved = await _notificationRepository.AddAsync(notification);
        var response = Map(saved);
        await DispatchAsync(response);
        return response;
    }

    public async Task<List<NotificationResponse>> SendBulkAsync(SendBulkNotificationsRequest request)
    {
        var errors = SendNotificationValidator.Validate(request);

        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var notifications = request.RecipientIds
            .Where(id => id > 0)
            .Distinct()
            .Select(id => new NotificationEntity
            {
                RecipientId = id,
                Type = request.Type,
                Title = request.Title.Trim(),
                Message = request.Message.Trim(),
                Channel = request.Channel,
                RelatedId = request.RelatedId,
                RelatedType = string.IsNullOrWhiteSpace(request.RelatedType) ? null : request.RelatedType.Trim(),
                IsRead = false,
                SentAt = DateTime.UtcNow
            })
            .ToList();

        if (notifications.Count == 0)
            throw new InvalidOperationException("At least one valid recipient is required.");

        await _notificationRepository.AddRangeAsync(notifications);

        var responses = notifications.Select(Map).ToList();

        foreach (var item in responses)
            await DispatchAsync(item);

        return responses;
    }

    public async Task<NotificationResponse> MarkAsReadAsync(int notificationId)
    {
        var notification = await _notificationRepository.FindByNotificationIdAsync(notificationId)
            ?? throw new KeyNotFoundException("Notification not found.");

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
        }

        return Map(notification);
    }

    public async Task<int> MarkAllReadAsync(int recipientId)
    {
        var items = await _notificationRepository.FindByRecipientIdAndIsReadAsync(recipientId, false);

        foreach (var item in items)
        {
            item.IsRead = true;
            await _notificationRepository.UpdateAsync(item);
        }

        return items.Count;
    }

    public async Task<List<NotificationResponse>> GetByRecipientAsync(int recipientId)
        => (await _notificationRepository.FindByRecipientIdAsync(recipientId)).Select(Map).ToList();

    public async Task<List<NotificationResponse>> GetUnreadByRecipientAsync(int recipientId)
        => (await _notificationRepository.FindByRecipientIdAndIsReadAsync(recipientId, false)).Select(Map).ToList();

    public Task<int> GetUnreadCountAsync(int recipientId)
        => _notificationRepository.CountByRecipientIdAndIsReadAsync(recipientId, false);

    public async Task DeleteNotificationAsync(int notificationId)
    {
        var notification = await _notificationRepository.FindByNotificationIdAsync(notificationId)
            ?? throw new KeyNotFoundException("Notification not found.");

        await _notificationRepository.DeleteByNotificationIdAsync(notification);
    }

    public Task SendEmailAsync(NotificationResponse notification)
    {
        return Task.CompletedTask;
    }

    public Task SendSmsAsync(NotificationResponse notification)
    {
        return Task.CompletedTask;
    }

    public async Task<List<NotificationResponse>> GetAllAsync()
        => (await _notificationRepository.GetAllAsync()).Select(Map).ToList();

    private async Task DispatchAsync(NotificationResponse notification)
    {
        switch (notification.Channel)
        {
            case NotificationChannel.App:
                await _realtimeDispatcher.SendToRecipientAsync(notification);
                break;
            case NotificationChannel.Email:
                await SendEmailAsync(notification);
                break;
            case NotificationChannel.Sms:
                await SendSmsAsync(notification);
                break;
            default:
                throw new InvalidOperationException("Unsupported notification channel.");
        }
    }

    private static NotificationResponse Map(NotificationEntity notification) => new()
    {
        NotificationId = notification.NotificationId,
        RecipientId = notification.RecipientId,
        Type = notification.Type,
        Title = notification.Title,
        Message = notification.Message,
        Channel = notification.Channel,
        RelatedId = notification.RelatedId,
        RelatedType = notification.RelatedType,
        IsRead = notification.IsRead,
        SentAt = notification.SentAt
    };
}
