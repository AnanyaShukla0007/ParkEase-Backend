using System.Net.Http.Json;
using Payment.Application.DTOs;
using Payment.Application.Interfaces;

namespace Payment.Infrastructure.Clients;

public class BookingPaymentClient : IBookingPaymentClient
{
    private readonly HttpClient _httpClient;

    public BookingPaymentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BookingLookupResponse?> GetBookingAsync(int bookingId)
    {
        var response = await _httpClient.GetAsync($"/api/v1/bookings/{bookingId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var envelope = await response.Content.ReadFromJsonAsync<BookingLookupEnvelope>();
        return envelope?.Data;
    }
}
