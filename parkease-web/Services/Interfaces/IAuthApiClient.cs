namespace ParkEase.Web.Services.Interfaces;

public interface IAuthApiClient
{
    Task<int> GetUserCountAsync();

    Task<int> GetManagerApplicationCountAsync(string status);
}
