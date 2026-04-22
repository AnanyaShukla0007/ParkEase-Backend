namespace Analytics.Application.Interfaces;

public interface IPaymentAnalyticsClient
{
    Task<int> GetSuccessfulPaymentCountAsync(int userId);
    Task<decimal> GetRevenueByLotAsync(int lotId, DateTime? from, DateTime? to);
    Task<List<(DateTime Day, decimal Revenue)>> GetRevenueByDayAsync(int lotId, DateTime? from, DateTime? to);
}
