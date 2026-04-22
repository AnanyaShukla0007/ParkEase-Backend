using Booking.Application.Interfaces;

namespace Booking.Infrastructure.Clients;

public class ParkingLotClient : IParkingLotClient
{
    private readonly HttpClient _httpClient;

    public ParkingLotClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DecrementAvailableAsync(int lotId)
    {
        var response = await _httpClient.PutAsync(
            $"/api/v1/parkinglots/{lotId}/decrement-available", null);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> IncrementAvailableAsync(int lotId)
    {
        var response = await _httpClient.PutAsync(
            $"/api/v1/parkinglots/{lotId}/increment-available", null);

        return response.IsSuccessStatusCode;
    }
}
