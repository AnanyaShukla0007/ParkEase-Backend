using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Analytics.Application.DTOs;
using Analytics.Application.Interfaces;

namespace Analytics.API.Controllers;

[ApiController]
[Route("api/v1/analytics")]
[Produces("application/json")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpPost("occupancy/log")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> LogOccupancy([FromBody] LogOccupancyRequest request)
    {
        var result = await _analyticsService.LogOccupancyAsync(request);
        return Ok(new { success = true, message = "Occupancy log created successfully.", data = result });
    }

    [HttpGet("occupancy-rate/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOccupancyRate(int lotId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetOccupancyRateAsync(lotId) });
    }

    [HttpGet("by-hour/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByHour(int lotId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetOccupancyByHourAsync(lotId) });
    }

    [HttpGet("peak-hours/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPeakHours(int lotId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetPeakHoursAsync(lotId) });
    }

    [HttpGet("revenue/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRevenueByLot(int lotId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(new { success = true, data = await _analyticsService.GetRevenueByLotAsync(lotId, from, to) });
    }

    [HttpGet("revenue-by-day/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRevenueByDay(int lotId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(new { success = true, data = await _analyticsService.GetRevenueByDayAsync(lotId, from, to) });
    }

    [HttpGet("spot-types/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSpotTypes(int lotId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetMostUsedSpotTypesAsync(lotId) });
    }

    [HttpGet("avg-duration/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvgDuration(int lotId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetAvgDurationAsync(lotId) });
    }

    [HttpGet("platform-summary")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPlatformSummary()
    {
        return Ok(new { success = true, data = await _analyticsService.GetPlatformSummaryAsync() });
    }

    [HttpGet("daily-report/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerateDailyReport(int lotId, [FromQuery] DateTime? date)
    {
        return Ok(new { success = true, data = await _analyticsService.GenerateDailyReportAsync(lotId, date) });
    }

    [HttpGet("users/{userId:int}/trust-score")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTrustScore(int userId)
    {
        return Ok(new { success = true, data = await _analyticsService.GetUserTrustScoreAsync(userId) });
    }

    [HttpGet("users/{userId:int}/carbon-savings")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCarbonSavings(int userId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(new { success = true, data = await _analyticsService.GetCarbonSavingsAsync(userId, from, to) });
    }

    [HttpPost("demand-signals")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> TrackDemandSignal([FromBody] TrackDemandSignalRequest request)
    {
        var result = await _analyticsService.TrackDemandSignalAsync(request);
        return Ok(new { success = true, message = "Demand signal tracked successfully.", data = result });
    }

    [HttpGet("demand/heatmap")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDemandHeatmap([FromQuery] string? city, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        return Ok(new { success = true, data = await _analyticsService.GetDemandHeatmapAsync(city, from, to) });
    }

    [HttpGet("demand/failed-hours")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFailedHours([FromQuery] string? city)
    {
        return Ok(new { success = true, data = await _analyticsService.GetFailedHoursAsync(city) });
    }
}
