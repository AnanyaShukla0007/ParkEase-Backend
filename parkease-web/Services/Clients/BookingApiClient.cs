using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class BookingApiClient : IBookingApiClient
{
    private readonly HttpClient _httpClient;

    public BookingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetBookingCountAsync(int userId) =>
        ApiResponseHelper.CountDataArrayAsync(_httpClient, $"/api/v1/bookings/user/{userId}");

    public Task<int> GetActiveBookingCountAsync() =>
        ApiResponseHelper.CountDataArrayAsync(_httpClient, "/api/v1/bookings/all");
}
