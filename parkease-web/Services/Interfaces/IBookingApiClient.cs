namespace ParkEase.Web.Services.Interfaces;

public interface IBookingApiClient
{
    Task<int> GetBookingCountAsync(int userId);
    Task<int> GetActiveBookingCountAsync();
}
