using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class SpotApiClient : ISpotApiClient
{
    private readonly HttpClient _httpClient;

    public SpotApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetAvailableSpotCountAsync(int lotId) =>
        ApiResponseHelper.CountDataArrayAsync(_httpClient, $"/api/v1/spots/lot/{lotId}/available");
}
