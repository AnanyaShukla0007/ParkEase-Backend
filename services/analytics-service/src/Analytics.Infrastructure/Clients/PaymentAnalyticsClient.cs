using System.Text.Json;
using Analytics.Application.DTOs;
using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class PaymentAnalyticsClient : IPaymentAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public PaymentAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetSuccessfulPaymentCountAsync(int userId)
        => (await GetPaymentsByUserAsync(userId)).Count(x => x.Status == 2);

    public Task<List<AnalyticsPaymentSnapshot>> GetPaymentsByBookingAsync(int bookingId)
        => GetPaymentsAsync($"/api/v1/payments/booking/{bookingId}");

    private Task<List<AnalyticsPaymentSnapshot>> GetPaymentsByUserAsync(int userId)
        => GetPaymentsAsync($"/api/v1/payments/user/{userId}");

    private async Task<List<AnalyticsPaymentSnapshot>> GetPaymentsAsync(string url)
    {
        using var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return [];

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        if (!document.RootElement.TryGetProperty("data", out var data) ||
            data.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var result = new List<AnalyticsPaymentSnapshot>();

        foreach (var item in data.EnumerateArray())
        {
            result.Add(new AnalyticsPaymentSnapshot
            {
                Id = item.TryGetProperty("id", out var id) ? id.GetInt32() : 0,
                BookingId = item.TryGetProperty("bookingId", out var bookingId) ? bookingId.GetInt32() : 0,
                UserId = item.TryGetProperty("userId", out var userId) ? userId.GetInt32() : 0,
                Amount = item.TryGetProperty("amount", out var amount) ? amount.GetDecimal() : 0,
                Status = item.TryGetProperty("status", out var status) ? status.GetInt32() : 0,
                CreatedAtUtc = item.TryGetProperty("createdAtUtc", out var created) ? created.GetDateTime() : default,
                PaidAtUtc = item.TryGetProperty("paidAtUtc", out var paidAt) && paidAt.ValueKind != JsonValueKind.Null ? paidAt.GetDateTime() : null
            });
        }

        return result;
    }
}
