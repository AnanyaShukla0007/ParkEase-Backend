namespace ParkEase.Web.Services.Interfaces;

public interface ISpotApiClient
{
    Task<int> GetAvailableSpotCountAsync(int lotId);
}
