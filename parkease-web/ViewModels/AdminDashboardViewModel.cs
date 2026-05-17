namespace ParkEase.Web.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int PendingManagerApplications { get; set; }
    public int ApprovedManagerApplications { get; set; }
    public int RejectedManagerApplications { get; set; }
    public int PendingBroadcasts { get; set; }
    public PlatformSummaryCardViewModel PlatformSummary { get; set; } = new();
    public List<DemandHeatmapCardViewModel> DemandHeatmap { get; set; } = [];
    public List<FailedHourCardViewModel> FailedHours { get; set; } = [];
}
