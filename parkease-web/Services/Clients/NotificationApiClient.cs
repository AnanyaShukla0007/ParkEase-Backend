using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class NotificationApiClient : INotificationApiClient
{
    private readonly HttpClient _httpClient;

    public NotificationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetUnreadCountAsync(int recipientId) => Task.FromResult(5);

    public Task<int> GetAdminBroadcastCountAsync() => Task.FromResult(2);
}
