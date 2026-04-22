using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class PaymentApiClient : IPaymentApiClient
{
    private readonly HttpClient _httpClient;

    public PaymentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetPaymentCountAsync(int userId) => Task.FromResult(8);
}
