using System.Text.Json;
using Analytics.Application.DTOs;
using Analytics.Application.Interfaces;

namespace Analytics.Infrastructure.Clients;

public class BookingAnalyticsClient : IBookingAnalyticsClient
{
    private readonly HttpClient _httpClient;

    public BookingAnalyticsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<int> GetCancellationCountAsync(int userId)
        => (await GetBookingsByUserAsync(userId)).Count(x => x.Status == 5);

    public async Task<int> GetNoShowCountAsync(int userId)
        => (await GetBookingsByUserAsync(userId)).Count(x => x.Status == 7);

    public async Task<int> GetOverstayCountAsync(int userId)
        => (await GetBookingsByUserAsync(userId)).Count(x =>
            x.CheckOutTimeUtc.HasValue && x.CheckOutTimeUtc.Value > x.EndTimeUtc);

    public async Task<int> GetOnTimeExitCountAsync(int userId)
        => (await GetBookingsByUserAsync(userId)).Count(x =>
            x.CheckOutTimeUtc.HasValue && x.CheckOutTimeUtc.Value <= x.EndTimeUtc);

    public async Task<decimal> GetAverageDurationByLotAsync(int lotId)
    {
        var bookings = await GetBookingsByLotAsync(lotId);
        var durations = bookings
            .Where(x => x.CheckInTimeUtc.HasValue && x.CheckOutTimeUtc.HasValue)
            .Select(x => (decimal)(x.CheckOutTimeUtc!.Value - x.CheckInTimeUtc!.Value).TotalHours)
            .ToList();

        return durations.Count == 0 ? 0 : decimal.Round(durations.Average(), 2);
    }

    public Task<List<AnalyticsBookingSnapshot>> GetBookingsByLotAsync(int lotId)
        => GetBookingsAsync($"/api/v1/bookings/lot/{lotId}");

    public Task<List<AnalyticsBookingSnapshot>> GetBookingsByUserAsync(int userId)
        => GetBookingsAsync($"/api/v1/bookings/user/{userId}");

    private async Task<List<AnalyticsBookingSnapshot>> GetBookingsAsync(string url)
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

        var result = new List<AnalyticsBookingSnapshot>();

        foreach (var item in data.EnumerateArray())
        {
            result.Add(new AnalyticsBookingSnapshot
            {
                Id = item.TryGetProperty("id", out var id) ? id.GetInt32() : 0,
                UserId = item.TryGetProperty("userId", out var userId) ? userId.GetInt32() : 0,
                LotId = item.TryGetProperty("lotId", out var lotId) ? lotId.GetInt32() : 0,
                SpotId = item.TryGetProperty("spotId", out var spotId) ? spotId.GetInt32() : 0,
                Status = item.TryGetProperty("status", out var status) ? status.GetInt32() : 0,
                StartTimeUtc = item.TryGetProperty("startTimeUtc", out var start) ? start.GetDateTime() : default,
                EndTimeUtc = item.TryGetProperty("endTimeUtc", out var end) ? end.GetDateTime() : default,
                CheckInTimeUtc = item.TryGetProperty("checkInTimeUtc", out var checkIn) && checkIn.ValueKind != JsonValueKind.Null ? checkIn.GetDateTime() : null,
                CheckOutTimeUtc = item.TryGetProperty("checkOutTimeUtc", out var checkOut) && checkOut.ValueKind != JsonValueKind.Null ? checkOut.GetDateTime() : null
            });
        }

        return result;
    }
}
