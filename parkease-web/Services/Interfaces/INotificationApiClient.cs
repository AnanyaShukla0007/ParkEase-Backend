namespace ParkEase.Web.Services.Interfaces;

public interface INotificationApiClient
{
    Task<int> GetUnreadCountAsync(int recipientId);
    Task<int> GetAdminBroadcastCountAsync();
}
