using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class SpotApiClient : ISpotApiClient
{
    private readonly HttpClient _httpClient;

    public SpotApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetAvailableSpotCountAsync(int lotId) => Task.FromResult(26);
}
