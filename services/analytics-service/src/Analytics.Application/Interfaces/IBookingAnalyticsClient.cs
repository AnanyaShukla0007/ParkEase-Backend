namespace Analytics.Application.Interfaces;

public interface IBookingAnalyticsClient
{
    Task<int> GetCancellationCountAsync(int userId);
    Task<int> GetNoShowCountAsync(int userId);
    Task<int> GetOverstayCountAsync(int userId);
    Task<int> GetOnTimeExitCountAsync(int userId);
    Task<decimal> GetAverageDurationByLotAsync(int lotId);
    Task<List<Analytics.Application.DTOs.AnalyticsBookingSnapshot>> GetBookingsByLotAsync(int lotId);
    Task<List<Analytics.Application.DTOs.AnalyticsBookingSnapshot>> GetBookingsByUserAsync(int userId);
}
