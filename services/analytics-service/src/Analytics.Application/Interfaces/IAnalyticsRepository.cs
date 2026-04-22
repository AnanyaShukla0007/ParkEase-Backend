using Analytics.Domain.Entities;

namespace Analytics.Application.Interfaces;

public interface IAnalyticsRepository
{
    Task<OccupancyLog> AddOccupancyLogAsync(OccupancyLog log);
    Task<List<OccupancyLog>> FindByLotIdAsync(int lotId);
    Task<List<OccupancyLog>> FindByLotIdAndTimestampBetweenAsync(int lotId, DateTime from, DateTime to);
    Task<decimal> AvgOccupancyByLotIdAsync(int lotId);
    Task<List<OccupancyLog>> FindPeakHoursByLotIdAsync(int lotId);
    Task<List<OccupancyLog>> FindByVehicleTypeAsync(string vehicleType);
    Task<int> CountByLotIdTodayAsync(int lotId);
    Task<List<int>> GetDistinctLotIdsAsync();
    Task<DemandSignal> AddDemandSignalAsync(DemandSignal signal);
    Task<List<DemandSignal>> GetDemandSignalsAsync(string? city, DateTime? from, DateTime? to);
}
