namespace Analytics.Application.Interfaces;

public interface IBookingAnalyticsClient
{
    Task<int> GetCancellationCountAsync(int userId);
    Task<int> GetNoShowCountAsync(int userId);
    Task<int> GetOverstayCountAsync(int userId);
    Task<int> GetOnTimeExitCountAsync(int userId);
    Task<decimal> GetAverageDurationByLotAsync(int lotId);
    Task<int> GetSpotUsageCountAsync(int lotId, string spotType);
}
