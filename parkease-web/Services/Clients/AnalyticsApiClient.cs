using System.Text.Json;
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

    public async Task<int> GetTrustScoreAsync(int userId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/analytics/users/{userId}/trust-score");

        if (data.HasValue &&
            data.Value.ValueKind == JsonValueKind.Object &&
            data.Value.TryGetProperty("score", out var score))
        {
            return score.GetInt32();
        }

        return 0;
    }

    public async Task<decimal> GetCarbonSavingsAsync(int userId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/analytics/users/{userId}/carbon-savings");

        if (data.HasValue &&
            data.Value.ValueKind == JsonValueKind.Object &&
            data.Value.TryGetProperty("co2ReducedKg", out var co2))
        {
            return co2.GetDecimal();
        }

        return 0;
    }

    public async Task<decimal> GetAverageOccupancyRateAsync(int lotId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/analytics/occupancy-rate/{lotId}");
        return data.HasValue && data.Value.ValueKind == JsonValueKind.Number
            ? data.Value.GetDecimal()
            : 0;
    }

    public async Task<decimal> GetRevenueSummaryAsync(int lotId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/analytics/revenue/{lotId}");

        if (data.HasValue &&
            data.Value.ValueKind == JsonValueKind.Object &&
            data.Value.TryGetProperty("totalRevenue", out var revenue))
        {
            return revenue.GetDecimal();
        }

        return 0;
    }

    public async Task<List<string>> GetPeakHoursAsync(int lotId)
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, $"/api/v1/analytics/peak-hours/{lotId}");
        var result = new List<string>();

        if (data.HasValue && data.Value.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in data.Value.EnumerateArray())
            {
                if (item.TryGetProperty("hourOfDay", out var hour))
                    result.Add($"{hour.GetInt32():00}:00 - {hour.GetInt32() + 1:00}:00");
            }
        }

        return result;
    }

    public async Task<PlatformSummaryCardViewModel> GetPlatformSummaryAsync()
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, "/api/v1/analytics/platform-summary");

        if (!data.HasValue || data.Value.ValueKind != JsonValueKind.Object)
            return new PlatformSummaryCardViewModel();

        return new PlatformSummaryCardViewModel
        {
            TotalLots = data.Value.TryGetProperty("totalLotsWithLogs", out var lots) ? lots.GetInt32() : 0,
            TotalRevenue = data.Value.TryGetProperty("totalRevenue", out var revenue) ? revenue.GetDecimal() : 0,
            AverageOccupancyRate = data.Value.TryGetProperty("averageOccupancyRate", out var occ) ? occ.GetDecimal() : 0
        };
    }

    public async Task<List<DemandHeatmapCardViewModel>> GetDemandHeatmapAsync()
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, "/api/v1/analytics/demand/heatmap");
        var result = new List<DemandHeatmapCardViewModel>();

        if (data.HasValue && data.Value.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in data.Value.EnumerateArray())
            {
                result.Add(new DemandHeatmapCardViewModel
                {
                    City = item.TryGetProperty("city", out var city) ? city.GetString() ?? string.Empty : string.Empty,
                    EventCount = item.TryGetProperty("eventCount", out var eventCount) ? eventCount.GetInt32() : 0,
                    NoResultCount = item.TryGetProperty("noResultCount", out var noResult) ? noResult.GetInt32() : 0,
                    FullLotCount = item.TryGetProperty("fullLotCount", out var fullLot) ? fullLot.GetInt32() : 0,
                    AbandonedSearchCount = item.TryGetProperty("abandonedSearchCount", out var abandoned) ? abandoned.GetInt32() : 0
                });
            }
        }

        return result;
    }

    public async Task<List<FailedHourCardViewModel>> GetFailedHoursAsync()
    {
        var data = await ApiResponseHelper.GetDataAsync(_httpClient, "/api/v1/analytics/demand/failed-hours");
        var result = new List<FailedHourCardViewModel>();

        if (data.HasValue && data.Value.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in data.Value.EnumerateArray())
            {
                result.Add(new FailedHourCardViewModel
                {
                    Hour = item.TryGetProperty("hourOfDay", out var hour) ? hour.GetInt32() : 0,
                    Count = item.TryGetProperty("failedEventCount", out var count) ? count.GetInt32() : 0
                });
            }
        }

        return result;
    }
}
