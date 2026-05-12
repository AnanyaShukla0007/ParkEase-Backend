using Microsoft.AspNetCore.Mvc;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var model = new LandingPageViewModel
        {
            Title = "Smart Parking for Drivers, Managers, and Admins",
            Highlights =
            [
                "Guest lot browsing and spot discovery",
                "Driver bookings, payments, notifications, and vehicle management",
                "Manager occupancy, revenue, and peak-hour insights",
                "Admin analytics, trust scores, carbon savings, and demand heatmaps"
            ]
        };

        return View(model);
    }
}
