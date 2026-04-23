using System.Text.Json;
using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class SpotAnalyticsClient : ISpotAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public SpotAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string?> GetSpotTypeAsync(int spotId)
    {
        using var response = await _httpClient.GetAsync($"/api/v1/spots/{spotId}");

        if (!response.IsSuccessStatusCode)
            return null;

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        if (document.RootElement.TryGetProperty("spotType", out var directSpotType))
            return directSpotType.ToString();

        if (!document.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object)
            return null;

        return data.TryGetProperty("spotType", out var nestedSpotType)
            ? nestedSpotType.ToString()
            : null;
    }
}
