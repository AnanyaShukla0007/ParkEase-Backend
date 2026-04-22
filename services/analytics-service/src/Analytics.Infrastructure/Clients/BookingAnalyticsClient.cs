using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class BookingAnalyticsClient : IBookingAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public BookingAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetCancellationCountAsync(int userId) => Task.FromResult(1);
    public Task<int> GetNoShowCountAsync(int userId) => Task.FromResult(0);
    public Task<int> GetOverstayCountAsync(int userId) => Task.FromResult(1);
    public Task<int> GetOnTimeExitCountAsync(int userId) => Task.FromResult(8);
    public Task<decimal> GetAverageDurationByLotAsync(int lotId) => Task.FromResult(2.75m);
    public Task<int> GetSpotUsageCountAsync(int lotId, string spotType) => Task.FromResult(spotType switch
    {
        "Standard" => 22,
        "Compact" => 10,
        "Large" => 7,
        "EV" => 5,
        "Handicapped" => 2,
        _ => 0
    });
}
