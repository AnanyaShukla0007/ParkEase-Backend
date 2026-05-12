using Microsoft.AspNetCore.Mvc;
using ParkEase.Web.ViewModels;

namespace ParkEase.Web.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login() => View(new LoginViewModel());

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpGet]
    public IActionResult Profile() => View(new ProfileViewModel
    {
        Name = "Demo User",
        Email = "demo@parkease.local",
        Role = "Driver"
    });
}
