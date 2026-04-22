using System.Net.Http.Json;
using Booking.Application.DTOs.Responses;
using Booking.Application.Interfaces;

namespace Booking.Infrastructure.Clients;

public class SpotClient : ISpotClient
{
    private readonly HttpClient _httpClient;

    public SpotClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ReserveSpotAsync(int spotId)
    {
        var response = await _httpClient.PutAsync(
            $"/api/v1/spots/{spotId}/reserve", null);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> OccupySpotAsync(int spotId)
    {
        var response = await _httpClient.PutAsync(
            $"/api/v1/spots/{spotId}/occupy", null);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReleaseSpotAsync(int spotId)
    {
        var response = await _httpClient.PutAsync(
            $"/api/v1/spots/{spotId}/release", null);

        return response.IsSuccessStatusCode;
    }

    public async Task<SpotLookupResponse?> GetSpotByIdAsync(int spotId)
    {
        return await _httpClient.GetFromJsonAsync<SpotLookupResponse>(
            $"/api/v1/spots/{spotId}");
    }
}
