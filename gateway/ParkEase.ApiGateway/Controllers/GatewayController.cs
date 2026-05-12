using Microsoft.AspNetCore.Mvc;

namespace ParkEase.ApiGateway.Controllers;

/// <summary>
/// Gateway health and info endpoints.
/// </summary>
[ApiController]
[Route("api/gateway")]
public class GatewayController : ControllerBase
{
    /// <summary>
    /// Check gateway status.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            success = true,
            service = "ParkEase API Gateway",
            status = "Running",
            time = DateTime.UtcNow
        });
    }
}