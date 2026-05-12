using Microsoft.EntityFrameworkCore;
using Analytics.Application.Interfaces;
using Analytics.Domain.Entities;
using Analytics.Infrastructure.Persistence;

namespace Analytics.Infrastructure.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly AnalyticsDbContext _context;

    public AnalyticsRepository(AnalyticsDbContext context)
    {
        _context = context;
    }

    public async Task<OccupancyLog> AddOccupancyLogAsync(OccupancyLog log)
    {
        await _context.OccupancyLogs.AddAsync(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<List<OccupancyLog>> FindByLotIdAsync(int lotId)
    {
        return await _context.OccupancyLogs
            .Where(x => x.LotId == lotId)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }

    public async Task<List<OccupancyLog>> FindByLotIdAndTimestampBetweenAsync(int lotId, DateTime from, DateTime to)
    {
        return await _context.OccupancyLogs
            .Where(x => x.LotId == lotId && x.Timestamp >= from && x.Timestamp <= to)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }

    public async Task<decimal> AvgOccupancyByLotIdAsync(int lotId)
    {
        var result = await _context.OccupancyLogs
            .Where(x => x.LotId == lotId)
            .Select(x => (decimal?)x.OccupancyRate)
            .AverageAsync();

        return decimal.Round(result ?? 0, 2);
    }

    public async Task<List<OccupancyLog>> FindPeakHoursByLotIdAsync(int lotId)
    {
        return await _context.OccupancyLogs
            .Where(x => x.LotId == lotId)
            .OrderByDescending(x => x.OccupancyRate)
            .Take(20)
            .ToListAsync();
    }

    public async Task<List<OccupancyLog>> FindByVehicleTypeAsync(string vehicleType)
    {
        return await _context.OccupancyLogs
            .Where(x => x.VehicleType == vehicleType)
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }

    public Task<int> CountByLotIdTodayAsync(int lotId)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        return _context.OccupancyLogs.CountAsync(x => x.LotId == lotId && x.Timestamp >= today && x.Timestamp < tomorrow);
    }

    public async Task<List<int>> GetDistinctLotIdsAsync()
    {
        return await _context.OccupancyLogs
            .Select(x => x.LotId)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();
    }

    public async Task<DemandSignal> AddDemandSignalAsync(DemandSignal signal)
    {
        await _context.DemandSignals.AddAsync(signal);
        await _context.SaveChangesAsync();
        return signal;
    }

    public async Task<List<DemandSignal>> GetDemandSignalsAsync(string? city, DateTime? from, DateTime? to)
    {
        var query = _context.DemandSignals.AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(x => x.City == city);
        if (from.HasValue)
            query = query.Where(x => x.OccurredAt >= from.Value);
        if (to.HasValue)
            query = query.Where(x => x.OccurredAt <= to.Value);

        return await query
            .OrderByDescending(x => x.OccurredAt)
            .ToListAsync();
    }
}
