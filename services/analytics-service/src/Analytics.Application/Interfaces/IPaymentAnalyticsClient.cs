namespace Analytics.Application.Interfaces;

public interface IPaymentAnalyticsClient
{
    Task<int> GetSuccessfulPaymentCountAsync(int userId);
    Task<List<Analytics.Application.DTOs.AnalyticsPaymentSnapshot>> GetPaymentsByBookingAsync(int bookingId);
}
