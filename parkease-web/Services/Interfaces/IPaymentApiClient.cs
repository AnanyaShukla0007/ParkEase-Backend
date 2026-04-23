namespace ParkEase.Web.Services.Interfaces;

public interface IPaymentApiClient
{
    Task<int> GetPaymentCountAsync(int userId);
}
