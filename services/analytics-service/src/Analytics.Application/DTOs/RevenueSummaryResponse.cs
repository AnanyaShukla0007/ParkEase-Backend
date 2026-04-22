namespace Analytics.Application.DTOs;

public class RevenueSummaryResponse
{
    public int LotId { get; set; }
    public decimal TotalRevenue { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
