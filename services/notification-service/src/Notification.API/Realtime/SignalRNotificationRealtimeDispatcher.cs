using Microsoft.AspNetCore.SignalR;
using Notification.API.Hubs;
using Notification.Application.DTOs;
using Notification.Application.Interfaces;

namespace Notification.API.Realtime;

public class SignalRNotificationRealtimeDispatcher : INotificationRealtimeDispatcher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationRealtimeDispatcher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendToRecipientAsync(NotificationResponse notification)
    {
        return _hubContext.Clients
            .Group(NotificationHub.GetGroupName(notification.RecipientId))
            .SendAsync("notificationReceived", notification);
    }
}
