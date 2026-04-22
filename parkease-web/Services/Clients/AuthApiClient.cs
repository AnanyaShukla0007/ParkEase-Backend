using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _httpClient;

    public AuthApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetUserCountAsync() => Task.FromResult(128);
}
