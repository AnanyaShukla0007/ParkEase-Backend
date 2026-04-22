using ParkEase.Web.Services.Interfaces;

namespace ParkEase.Web.Services.Clients;

public class BookingApiClient : IBookingApiClient
{
    private readonly HttpClient _httpClient;

    public BookingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<int> GetBookingCountAsync(int userId) => Task.FromResult(12);

    public Task<int> GetActiveBookingCountAsync() => Task.FromResult(9);
}
