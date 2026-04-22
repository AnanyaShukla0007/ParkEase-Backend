namespace ParkEase.Web.ViewModels;

public class DriverDashboardViewModel
{
    public int VehicleCount { get; set; }
    public int BookingCount { get; set; }
    public int PaymentCount { get; set; }
    public int UnreadNotifications { get; set; }
    public int TrustScore { get; set; }
    public decimal CarbonSavings { get; set; }
}
