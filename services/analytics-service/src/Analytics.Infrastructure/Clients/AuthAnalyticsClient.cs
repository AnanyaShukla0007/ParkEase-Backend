using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class AuthAnalyticsClient : IAuthAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public AuthAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<bool> UserExistsAsync(int userId) => Task.FromResult(userId > 0);
}
