using System.Text.Json;

namespace ParkEase.Web.Services.Clients;

internal static class ApiResponseHelper
{
    public static async Task<int> CountDataArrayAsync(HttpClient httpClient, string url)
    {
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        if (document.RootElement.TryGetProperty("data", out var data) &&
            data.ValueKind == JsonValueKind.Array)
        {
            return data.GetArrayLength();
        }

        return 0;
    }

    public static async Task<JsonElement?> GetDataAsync(HttpClient httpClient, string url)
    {
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        if (!document.RootElement.TryGetProperty("data", out var data))
            return null;

        return data.Clone();
    }
}
