using Microsoft.AspNetCore.SignalR;

namespace Notification.API.Hubs;

public class NotificationHub : Hub
{
    public async Task JoinRecipientGroup(int recipientId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(recipientId));
    }

    public async Task LeaveRecipientGroup(int recipientId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(recipientId));
    }

    public static string GetGroupName(int recipientId) => $"recipient-{recipientId}";
}
