namespace ParkEase.Web.Services.Interfaces;

public interface IParkingLotApiClient
{
    Task<int> GetManagedLotCountAsync();
}
