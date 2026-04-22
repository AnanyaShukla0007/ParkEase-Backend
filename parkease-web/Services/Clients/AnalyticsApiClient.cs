using ParkEase.Web.Services.Interfaces;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Services.Clients;

public class AnalyticsApiClient : IAnalyticsApiClient
{
    private readonly HttpClient _httpClient;

    public AnalyticsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetTrustScoreAsync(int userId) => Task.FromResult(82);

    public Task<decimal> GetCarbonSavingsAsync(int userId) => Task.FromResult(3.1m);

    public Task<decimal> GetAverageOccupancyRateAsync(int lotId) => Task.FromResult(74.5m);

    public Task<decimal> GetRevenueSummaryAsync(int lotId) => Task.FromResult(18450.50m);

    public Task<List<string>> GetPeakHoursAsync(int lotId) =>
        Task.FromResult(new List<string> { "10:00 - 11:00", "13:00 - 14:00", "18:00 - 19:00" });

    public Task<PlatformSummaryCardViewModel> GetPlatformSummaryAsync() =>
        Task.FromResult(new PlatformSummaryCardViewModel
        {
            TotalLots = 6,
            TotalRevenue = 75420.25m,
            AverageOccupancyRate = 71.8m
        });

    public Task<List<DemandHeatmapCardViewModel>> GetDemandHeatmapAsync() =>
        Task.FromResult(new List<DemandHeatmapCardViewModel>
        {
            new() { City = "Lucknow", EventCount = 14, NoResultCount = 5, FullLotCount = 6, AbandonedSearchCount = 3 },
            new() { City = "Kanpur", EventCount = 9, NoResultCount = 2, FullLotCount = 4, AbandonedSearchCount = 3 }
        });

    public Task<List<FailedHourCardViewModel>> GetFailedHoursAsync() =>
        Task.FromResult(new List<FailedHourCardViewModel>
        {
            new() { Hour = 18, Count = 8 },
            new() { Hour = 19, Count = 6 },
            new() { Hour = 9, Count = 4 }
        });
}
