namespace Analytics.Application.DTOs;

public class CarbonSavingsResponse
{
    public int UserId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal TimeSavedMinutes { get; set; }
    public decimal AvgIdleEmissionRate { get; set; }
    public decimal Co2ReducedKg { get; set; }
}
