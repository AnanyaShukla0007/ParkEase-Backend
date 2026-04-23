using System.Text.Json;
using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetUserCountAsync()
    {
        using var response = await _httpClient.GetAsync("/api/v1/auth/users/count");
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        return document.RootElement.TryGetProperty("count", out var count)
            ? count.GetInt32()
            : 0;
    }
}
