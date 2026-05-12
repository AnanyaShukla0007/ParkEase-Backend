namespace Analytics.Application.DTOs;

public class DailyReportResponse
{
    public int LotId { get; set; }
    public DateTime ReportDate { get; set; }
    public decimal AverageOccupancyRate { get; set; }
    public decimal Revenue { get; set; }
    public List<PeakHourResponse> PeakHours { get; set; } = [];
    public List<SpotTypeUsageResponse> SpotTypeUsage { get; set; } = [];
}
