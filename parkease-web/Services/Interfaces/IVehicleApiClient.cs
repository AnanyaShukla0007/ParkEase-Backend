namespace ParkEase.Web.Services.Interfaces;

public interface IVehicleApiClient
{
    Task<int> GetVehicleCountAsync(int ownerId);
}
