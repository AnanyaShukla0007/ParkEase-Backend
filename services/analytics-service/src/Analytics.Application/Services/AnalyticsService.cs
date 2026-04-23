using Microsoft.Extensions.Configuration;
using Analytics.Application.DTOs;
using Analytics.Application.Interfaces;
using Analytics.Application.Validators;
using Analytics.Domain.Entities;

namespace Analytics.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IAnalyticsRepository _analyticsRepository;
    private readonly IBookingAnalyticsClient _bookingClient;
    private readonly IPaymentAnalyticsClient _paymentClient;
    private readonly IAuthAnalyticsClient _authClient;
    private readonly ISpotAnalyticsClient _spotClient;
    private readonly IConfiguration _configuration;

    public AnalyticsService(
        IAnalyticsRepository analyticsRepository,
        IBookingAnalyticsClient bookingClient,
        IPaymentAnalyticsClient paymentClient,
        IAuthAnalyticsClient authClient,
        ISpotAnalyticsClient spotClient,
        IConfiguration configuration)
    {
        _analyticsRepository = analyticsRepository;
        _bookingClient = bookingClient;
        _paymentClient = paymentClient;
        _authClient = authClient;
        _spotClient = spotClient;
        _configuration = configuration;
    }

    public async Task<OccupancyLogResponse> LogOccupancyAsync(LogOccupancyRequest request)
    {
        var errors = AnalyticsValidator.Validate(request);
        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var entity = new OccupancyLog
        {
            LotId = request.LotId,
            SpotId = request.SpotId,
            Timestamp = request.Timestamp == default ? DateTime.UtcNow : request.Timestamp,
            OccupancyRate = request.OccupancyRate,
            AvailableSpots = request.AvailableSpots,
            TotalSpots = request.TotalSpots,
            VehicleType = request.VehicleType.Trim()
        };

        var saved = await _analyticsRepository.AddOccupancyLogAsync(entity);
        return Map(saved);
    }

    public Task<decimal> GetOccupancyRateAsync(int lotId)
        => _analyticsRepository.AvgOccupancyByLotIdAsync(lotId);

    public async Task<List<HourlyOccupancyResponse>> GetOccupancyByHourAsync(int lotId)
    {
        var items = await _analyticsRepository.FindByLotIdAsync(lotId);

        return items
            .GroupBy(x => x.Timestamp.Hour)
            .Select(g => new HourlyOccupancyResponse
            {
                HourOfDay = g.Key,
                AverageOccupancyRate = decimal.Round(g.Average(x => x.OccupancyRate), 2)
            })
            .OrderBy(x => x.HourOfDay)
            .ToList();
    }

    public async Task<List<PeakHourResponse>> GetPeakHoursAsync(int lotId)
    {
        var items = await _analyticsRepository.FindPeakHoursByLotIdAsync(lotId);

        return items
            .GroupBy(x => x.Timestamp.Hour)
            .Select(g => new PeakHourResponse
            {
                HourOfDay = g.Key,
                AverageOccupancyRate = decimal.Round(g.Average(x => x.OccupancyRate), 2)
            })
            .OrderByDescending(x => x.AverageOccupancyRate)
            .Take(3)
            .ToList();
    }

    public async Task<RevenueSummaryResponse> GetRevenueByLotAsync(int lotId, DateTime? from, DateTime? to)
    {
        var targetFrom = from ?? DateTime.MinValue;
        var targetTo = to ?? DateTime.MaxValue;
        var bookings = await _bookingClient.GetBookingsByLotAsync(lotId);
        decimal totalRevenue = 0;

        foreach (var booking in bookings)
        {
            var payments = await _paymentClient.GetPaymentsByBookingAsync(booking.Id);
            totalRevenue += payments
                .Where(x => x.Status == 2)
                .Where(x => (x.PaidAtUtc ?? x.CreatedAtUtc) >= targetFrom && (x.PaidAtUtc ?? x.CreatedAtUtc) <= targetTo)
                .Sum(x => x.Amount);
        }

        return new RevenueSummaryResponse
        {
            LotId = lotId,
            TotalRevenue = decimal.Round(totalRevenue, 2),
            From = from,
            To = to
        };
    }

    public async Task<List<RevenueByDayResponse>> GetRevenueByDayAsync(int lotId, DateTime? from, DateTime? to)
    {
        var targetFrom = from ?? DateTime.MinValue;
        var targetTo = to ?? DateTime.MaxValue;
        var bookings = await _bookingClient.GetBookingsByLotAsync(lotId);
        var payments = new List<AnalyticsPaymentSnapshot>();

        foreach (var booking in bookings)
            payments.AddRange(await _paymentClient.GetPaymentsByBookingAsync(booking.Id));

        return payments
            .Where(x => x.Status == 2)
            .Select(x => new { Day = (x.PaidAtUtc ?? x.CreatedAtUtc).Date, x.Amount })
            .Where(x => x.Day >= targetFrom.Date && x.Day <= targetTo.Date)
            .GroupBy(x => x.Day)
            .Select(g => new RevenueByDayResponse { Day = g.Key, Revenue = decimal.Round(g.Sum(x => x.Amount), 2) })
            .OrderBy(x => x.Day)
            .ToList();
    }

    public async Task<List<SpotTypeUsageResponse>> GetMostUsedSpotTypesAsync(int lotId)
    {
        var bookings = await _bookingClient.GetBookingsByLotAsync(lotId);
        var usage = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var booking in bookings)
        {
            var spotType = await _spotClient.GetSpotTypeAsync(booking.SpotId) ?? "Unknown";
            usage[spotType] = usage.TryGetValue(spotType, out var count) ? count + 1 : 1;
        }

        return usage
            .Select(x => new SpotTypeUsageResponse
            {
                SpotType = x.Key,
                Count = x.Value
            })
            .OrderByDescending(x => x.Count)
            .ToList();
    }

    public Task<decimal> GetAvgDurationAsync(int lotId)
        => _bookingClient.GetAverageDurationByLotAsync(lotId);

    public async Task<PlatformSummaryResponse> GetPlatformSummaryAsync()
    {
        var lotIds = await _analyticsRepository.GetDistinctLotIdsAsync();
        decimal totalRevenue = 0;
        decimal avgOcc = 0;
        var totalLogsToday = 0;

        foreach (var lotId in lotIds)
        {
            totalRevenue += (await GetRevenueByLotAsync(lotId, null, null)).TotalRevenue;
            avgOcc += await _analyticsRepository.AvgOccupancyByLotIdAsync(lotId);
            totalLogsToday += await _analyticsRepository.CountByLotIdTodayAsync(lotId);
        }

        return new PlatformSummaryResponse
        {
            TotalLotsWithLogs = lotIds.Count,
            TotalLogsToday = totalLogsToday,
            AverageOccupancyRate = lotIds.Count == 0 ? 0 : decimal.Round(avgOcc / lotIds.Count, 2),
            TotalRevenue = decimal.Round(totalRevenue, 2)
        };
    }

    public async Task<DailyReportResponse> GenerateDailyReportAsync(int lotId, DateTime? date)
    {
        var targetDate = (date ?? DateTime.UtcNow).Date;
        var from = targetDate;
        var to = targetDate.AddDays(1).AddTicks(-1);
        var logs = await _analyticsRepository.FindByLotIdAndTimestampBetweenAsync(lotId, from, to);
        var revenues = await GetRevenueByLotAsync(lotId, from, to);

        return new DailyReportResponse
        {
            LotId = lotId,
            ReportDate = targetDate,
            AverageOccupancyRate = logs.Count == 0 ? 0 : decimal.Round(logs.Average(x => x.OccupancyRate), 2),
            Revenue = revenues.TotalRevenue,
            PeakHours = await GetPeakHoursAsync(lotId),
            SpotTypeUsage = await GetMostUsedSpotTypesAsync(lotId)
        };
    }

    public async Task<UserTrustScoreResponse> GetUserTrustScoreAsync(int userId)
    {
        if (!await _authClient.UserExistsAsync(userId))
            throw new KeyNotFoundException("User not found.");

        var cancellations = await _bookingClient.GetCancellationCountAsync(userId);
        var noShows = await _bookingClient.GetNoShowCountAsync(userId);
        var overstays = await _bookingClient.GetOverstayCountAsync(userId);
        var onTimeExits = await _bookingClient.GetOnTimeExitCountAsync(userId);
        var successfulPayments = await _paymentClient.GetSuccessfulPaymentCountAsync(userId);

        var baseScore = _configuration.GetValue<int>("AnalyticsSettings:TrustScore:BaseScore", 100);
        var score = baseScore
            - cancellations * _configuration.GetValue<int>("AnalyticsSettings:TrustScore:CancellationPenalty", 5)
            - noShows * _configuration.GetValue<int>("AnalyticsSettings:TrustScore:NoShowPenalty", 15)
            - overstays * _configuration.GetValue<int>("AnalyticsSettings:TrustScore:OverstayPenalty", 7)
            + onTimeExits * _configuration.GetValue<int>("AnalyticsSettings:TrustScore:OnTimeExitReward", 2)
            + successfulPayments * _configuration.GetValue<int>("AnalyticsSettings:TrustScore:SuccessfulPaymentReward", 1);

        score = Math.Max(0, Math.Min(100, score));

        return new UserTrustScoreResponse
        {
            UserId = userId,
            Score = score,
            Tier = score >= 85 ? "Gold" : score >= 70 ? "Silver" : "Bronze",
            CancellationCount = cancellations,
            NoShowCount = noShows,
            OverstayCount = overstays,
            OnTimeExitCount = onTimeExits,
            SuccessfulPaymentCount = successfulPayments,
            CalculatedAt = DateTime.UtcNow
        };
    }

    public async Task<CarbonSavingsResponse> GetCarbonSavingsAsync(int userId, DateTime? from, DateTime? to)
    {
        if (!await _authClient.UserExistsAsync(userId))
            throw new KeyNotFoundException("User not found.");

        var targetFrom = (from ?? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1)).Date;
        var targetTo = (to ?? DateTime.UtcNow).Date.AddDays(1).AddTicks(-1);

        var signals = await _analyticsRepository.GetDemandSignalsAsync(null, targetFrom, targetTo);
        var userSignals = signals.Where(x => x.UserId == userId).ToList();
        var timeSavedMinutes = userSignals.Count(x => x.EventType.Equals("SEARCH_SUCCESS", StringComparison.OrdinalIgnoreCase)) * 6.5m;
        var emissionRate = _configuration.GetValue<decimal>("AnalyticsSettings:AvgIdleEmissionRateKgPerMinute", 0.12m);

        return new CarbonSavingsResponse
        {
            UserId = userId,
            PeriodStart = targetFrom,
            PeriodEnd = targetTo,
            TimeSavedMinutes = decimal.Round(timeSavedMinutes, 2),
            AvgIdleEmissionRate = emissionRate,
            Co2ReducedKg = decimal.Round(timeSavedMinutes * emissionRate, 2)
        };
    }

    public async Task<DemandSignalResponse> TrackDemandSignalAsync(TrackDemandSignalRequest request)
    {
        var errors = AnalyticsValidator.Validate(request);
        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var entity = new DemandSignal
        {
            UserId = request.UserId,
            City = request.City.Trim(),
            LotId = request.LotId,
            EventType = request.EventType.Trim().ToUpperInvariant(),
            SearchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? null : request.SearchTerm.Trim(),
            Reason = string.IsNullOrWhiteSpace(request.Reason) ? null : request.Reason.Trim(),
            OccurredAt = request.OccurredAt ?? DateTime.UtcNow
        };

        var saved = await _analyticsRepository.AddDemandSignalAsync(entity);
        return Map(saved);
    }

    public async Task<List<DemandHeatmapResponse>> GetDemandHeatmapAsync(string? city, DateTime? from, DateTime? to)
    {
        var signals = await _analyticsRepository.GetDemandSignalsAsync(city, from, to);

        return signals
            .GroupBy(x => x.City)
            .Select(g => new DemandHeatmapResponse
            {
                City = g.Key,
                EventCount = g.Count(),
                NoResultCount = g.Count(x => x.EventType == "NO_RESULTS"),
                FullLotCount = g.Count(x => x.EventType == "FULL_LOTS"),
                AbandonedSearchCount = g.Count(x => x.EventType == "ABANDONED_SEARCH")
            })
            .OrderByDescending(x => x.EventCount)
            .ToList();
    }

    public async Task<List<FailedHourResponse>> GetFailedHoursAsync(string? city)
    {
        var signals = await _analyticsRepository.GetDemandSignalsAsync(city, null, null);
        var failed = signals.Where(x => x.EventType is "NO_RESULTS" or "FULL_LOTS" or "ABANDONED_SEARCH");

        return failed
            .GroupBy(x => x.OccurredAt.Hour)
            .Select(g => new FailedHourResponse
            {
                HourOfDay = g.Key,
                FailedEventCount = g.Count()
            })
            .OrderByDescending(x => x.FailedEventCount)
            .ToList();
    }

    private static OccupancyLogResponse Map(OccupancyLog log) => new()
    {
        LogId = log.LogId,
        LotId = log.LotId,
        SpotId = log.SpotId,
        Timestamp = log.Timestamp,
        OccupancyRate = log.OccupancyRate,
        AvailableSpots = log.AvailableSpots,
        TotalSpots = log.TotalSpots,
        VehicleType = log.VehicleType
    };

    private static DemandSignalResponse Map(DemandSignal signal) => new()
    {
        Id = signal.Id,
        UserId = signal.UserId,
        City = signal.City,
        LotId = signal.LotId,
        EventType = signal.EventType,
        SearchTerm = signal.SearchTerm,
        Reason = signal.Reason,
        OccurredAt = signal.OccurredAt
    };
}
