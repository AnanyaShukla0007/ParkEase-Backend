using Analytics.Domain.Entities;

namespace Analytics.Infrastructure.Persistence.Seed;

public static class AnalyticsSeeder
{
    public static async Task SeedAsync(AnalyticsDbContext context)
    {
        if (!context.OccupancyLogs.Any())
        {
            await context.OccupancyLogs.AddRangeAsync(new List<OccupancyLog>
            {
                new() { LotId = 1, SpotId = 101, Timestamp = DateTime.UtcNow.AddHours(-3), OccupancyRate = 62.5m, AvailableSpots = 15, TotalSpots = 40, VehicleType = "4W" },
                new() { LotId = 1, SpotId = 102, Timestamp = DateTime.UtcNow.AddHours(-2), OccupancyRate = 78.0m, AvailableSpots = 9, TotalSpots = 40, VehicleType = "4W" },
                new() { LotId = 2, SpotId = 201, Timestamp = DateTime.UtcNow.AddHours(-1), OccupancyRate = 55.0m, AvailableSpots = 18, TotalSpots = 40, VehicleType = "2W" }
            });
        }

        if (!context.DemandSignals.Any())
        {
            await context.DemandSignals.AddRangeAsync(new List<DemandSignal>
            {
                new() { UserId = 1, City = "Lucknow", LotId = null, EventType = "NO_RESULTS", SearchTerm = "Hazratganj", Reason = "No lots returned", OccurredAt = DateTime.UtcNow.AddDays(-1) },
                new() { UserId = 2, City = "Lucknow", LotId = 4, EventType = "FULL_LOTS", SearchTerm = "Aliganj", Reason = "All nearby lots full", OccurredAt = DateTime.UtcNow.AddHours(-5) },
                new() { UserId = 1, City = "Lucknow", LotId = 3, EventType = "SEARCH_SUCCESS", SearchTerm = "Gomti Nagar", Reason = null, OccurredAt = DateTime.UtcNow.AddHours(-2) }
            });
        }

        await context.SaveChangesAsync();
    }
}
