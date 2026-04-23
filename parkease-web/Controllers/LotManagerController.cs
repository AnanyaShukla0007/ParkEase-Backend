using Microsoft.AspNetCore.Mvc;
using ParkEase.Web.Services.Interfaces;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Controllers;

public class LotManagerController : Controller
{
    private readonly IParkingLotApiClient _parkingLotClient;
    private readonly IBookingApiClient _bookingClient;
    private readonly IAnalyticsApiClient _analyticsClient;

    public LotManagerController(
        IParkingLotApiClient parkingLotClient,
        IBookingApiClient bookingClient,
        IAnalyticsApiClient analyticsClient)
    {
        _parkingLotClient = parkingLotClient;
        _bookingClient = bookingClient;
        _analyticsClient = analyticsClient;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new LotManagerDashboardViewModel
        {
            LotCount = await _parkingLotClient.GetManagedLotCountAsync(),
            ActiveBookings = await _bookingClient.GetActiveBookingCountAsync(),
            AverageOccupancyRate = await _analyticsClient.GetAverageOccupancyRateAsync(1),
            Revenue = await _analyticsClient.GetRevenueSummaryAsync(1),
            PeakHours = await _analyticsClient.GetPeakHoursAsync(1)
        };

        return View(model);
    }
}
