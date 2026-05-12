namespace ParkEase.Web.ViewModels;

public class LotManagerDashboardViewModel
{
    public int LotCount { get; set; }
    public int ActiveBookings { get; set; }
    public decimal AverageOccupancyRate { get; set; }
    public decimal Revenue { get; set; }
    public List<string> PeakHours { get; set; } = [];
}
