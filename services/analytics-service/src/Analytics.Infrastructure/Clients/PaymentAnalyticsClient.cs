using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class PaymentAnalyticsClient : IPaymentAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public PaymentAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetSuccessfulPaymentCountAsync(int userId) => Task.FromResult(14);

    public Task<decimal> GetRevenueByLotAsync(int lotId, DateTime? from, DateTime? to)
        => Task.FromResult(18450.50m);

    public Task<List<(DateTime Day, decimal Revenue)>> GetRevenueByDayAsync(int lotId, DateTime? from, DateTime? to)
        => Task.FromResult(new List<(DateTime Day, decimal Revenue)>
        {
            (DateTime.UtcNow.Date.AddDays(-2), 5200m),
            (DateTime.UtcNow.Date.AddDays(-1), 6300.50m),
            (DateTime.UtcNow.Date, 6950m)
        });
}
