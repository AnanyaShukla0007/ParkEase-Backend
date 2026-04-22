namespace Analytics.Application.DTOs;

public class PlatformSummaryResponse
{
    public int TotalLotsWithLogs { get; set; }
    public int TotalLogsToday { get; set; }
    public decimal AverageOccupancyRate { get; set; }
    public decimal TotalRevenue { get; set; }
}
