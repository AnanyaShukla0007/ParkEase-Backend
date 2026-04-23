using Microsoft.AspNetCore.Mvc;
using ParkEase.Web.Services.Interfaces;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Controllers;

public class DriverController : Controller
{
    private readonly IVehicleApiClient _vehicleClient;
    private readonly IBookingApiClient _bookingClient;
    private readonly IPaymentApiClient _paymentClient;
    private readonly INotificationApiClient _notificationClient;
    private readonly IAnalyticsApiClient _analyticsClient;

    public DriverController(
        IVehicleApiClient vehicleClient,
        IBookingApiClient bookingClient,
        IPaymentApiClient paymentClient,
        INotificationApiClient notificationClient,
        IAnalyticsApiClient analyticsClient)
    {
        _vehicleClient = vehicleClient;
        _bookingClient = bookingClient;
        _paymentClient = paymentClient;
        _notificationClient = notificationClient;
        _analyticsClient = analyticsClient;
    }

    public async Task<IActionResult> Dashboard()
    {
        var model = new DriverDashboardViewModel
        {
            VehicleCount = await _vehicleClient.GetVehicleCountAsync(1),
            BookingCount = await _bookingClient.GetBookingCountAsync(1),
            PaymentCount = await _paymentClient.GetPaymentCountAsync(1),
            UnreadNotifications = await _notificationClient.GetUnreadCountAsync(1),
            TrustScore = await _analyticsClient.GetTrustScoreAsync(1),
            CarbonSavings = await _analyticsClient.GetCarbonSavingsAsync(1)
        };

        return View(model);
    }
}
