using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class ParkingLotApiClient : IParkingLotApiClient
{
    private readonly HttpClient _httpClient;

    public ParkingLotApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetManagedLotCountAsync() => ApiResponseHelper.CountDataArrayAsync(_httpClient, "/api/v1/parkinglots");
}
