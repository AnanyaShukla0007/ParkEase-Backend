using Analytics.Application.DTOs;

namespace Analytics.Application.Interfaces;

public interface IAnalyticsService
{
    Task<OccupancyLogResponse> LogOccupancyAsync(LogOccupancyRequest request);
    Task<decimal> GetOccupancyRateAsync(int lotId);
    Task<List<HourlyOccupancyResponse>> GetOccupancyByHourAsync(int lotId);
    Task<List<PeakHourResponse>> GetPeakHoursAsync(int lotId);
    Task<RevenueSummaryResponse> GetRevenueByLotAsync(int lotId, DateTime? from, DateTime? to);
    Task<List<RevenueByDayResponse>> GetRevenueByDayAsync(int lotId, DateTime? from, DateTime? to);
    Task<List<SpotTypeUsageResponse>> GetMostUsedSpotTypesAsync(int lotId);
    Task<decimal> GetAvgDurationAsync(int lotId);
    Task<PlatformSummaryResponse> GetPlatformSummaryAsync();
    Task<DailyReportResponse> GenerateDailyReportAsync(int lotId, DateTime? date);
    Task<UserTrustScoreResponse> GetUserTrustScoreAsync(int userId);
    Task<CarbonSavingsResponse> GetCarbonSavingsAsync(int userId, DateTime? from, DateTime? to);
    Task<DemandSignalResponse> TrackDemandSignalAsync(TrackDemandSignalRequest request);
    Task<List<DemandHeatmapResponse>> GetDemandHeatmapAsync(string? city, DateTime? from, DateTime? to);
    Task<List<FailedHourResponse>> GetFailedHoursAsync(string? city);
}
