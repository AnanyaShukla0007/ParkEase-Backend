using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class VehicleApiClient : IVehicleApiClient
{
    private readonly HttpClient _httpClient;

    public VehicleApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetVehicleCountAsync(int ownerId) =>
        ApiResponseHelper.CountDataArrayAsync(_httpClient, $"/api/v1/vehicles/owner/{ownerId}");
}
