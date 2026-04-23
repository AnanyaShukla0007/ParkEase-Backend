using System.Text.Json;
using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class AuthAnalyticsClient : IAuthAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public AuthAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        using var response = await _httpClient.GetAsync($"/api/v1/auth/users/{userId}/exists");

        if (!response.IsSuccessStatusCode)
            return false;

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        return document.RootElement.TryGetProperty("exists", out var exists) && exists.GetBoolean();
    }
}
