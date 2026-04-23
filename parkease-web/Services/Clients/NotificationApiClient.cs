using System.Text.Json;
using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class NotificationApiClient : INotificationApiClient
{
    private readonly HttpClient _httpClient;

    public NotificationApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetUnreadCountAsync(int recipientId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/notifications/recipient/{recipientId}/unread-count");
        return data.HasValue && data.Value.ValueKind == JsonValueKind.Number
            ? data.Value.GetInt32()
            : 0;
    }

    public Task<int> GetAdminBroadcastCountAsync() =>
        ApiResponseHelper.CountDataArrayAsync(_httpClient, "/api/v1/notifications/all");
}
