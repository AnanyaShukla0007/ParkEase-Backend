using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Services.Interfaces;

public interface IAnalyticsApiClient
{
    Task<int> GetTrustScoreAsync(int userId);
    Task<decimal> GetCarbonSavingsAsync(int userId);
    Task<decimal> GetAverageOccupancyRateAsync(int lotId);
    Task<decimal> GetRevenueSummaryAsync(int lotId);
    Task<List<string>> GetPeakHoursAsync(int lotId);
    Task<PlatformSummaryCardViewModel> GetPlatformSummaryAsync();
    Task<List<DemandHeatmapCardViewModel>> GetDemandHeatmapAsync();
    Task<List<FailedHourCardViewModel>> GetFailedHoursAsync();
}
