using Microsoft.AspNetCore.Mvc;
using ParkEase.Web.Services.Interfaces;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Controllers;

public class AdminController : Controller
{
    private readonly IAuthApiClient _authClient;
    private readonly IAnalyticsApiClient _analyticsClient;
    private readonly INotificationApiClient _notificationClient;

    public AdminController(
        IAuthApiClient authClient,
        IAnalyticsApiClient analyticsClient,
        INotificationApiClient notificationClient)
    {
        _authClient = authClient;
        _analyticsClient = analyticsClient;
        _notificationClient = notificationClient;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new AdminDashboardViewModel
        {
            TotalUsers = await _authClient.GetUserCountAsync(),
            PlatformSummary = await _analyticsClient.GetPlatformSummaryAsync(),
            DemandHeatmap = await _analyticsClient.GetDemandHeatmapAsync(),
            FailedHours = await _analyticsClient.GetFailedHoursAsync(),
            PendingBroadcasts = await _notificationClient.GetAdminBroadcastCountAsync()
        };

        return View(model);
    }
}
