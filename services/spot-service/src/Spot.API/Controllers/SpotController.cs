using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spot.Application.Interfaces;
using ParkEase.Spot.Application.DTOs;

namespace Spot.API.Controllers;

[ApiController]
[Route("api/v1/spots")]
[Produces("application/json")]
public class SpotsController : ControllerBase
{
    private readonly ISpotService _service;

    public SpotsController(ISpotService service)
    {
        _service = service;
    }

    /// <summary>Get all parking spots</summary>
    /// <remarks>Access: Public (Guest, Driver, Manager, Admin)</remarks>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get parking spot by ID</summary>
    /// <remarks>Access: Public</remarks>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    /// <summary>Get all spots by parking lot</summary>
    /// <remarks>Access: Public</remarks>
    [HttpGet("lot/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByLot(int lotId)
        => Ok(await _service.GetByLotAsync(lotId));

    /// <summary>Get available spots by parking lot</summary>
    /// <remarks>Access: Public</remarks>
    [HttpGet("lot/{lotId:int}/available")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailable(int lotId)
        => Ok(await _service.GetAvailableByLotAsync(lotId));

    /// <summary>Create new parking spot</summary>
    /// <remarks>Access: MANAGER, ADMIN</remarks>
    [HttpPost]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreateSpotRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    /// <summary>Bulk create parking spots</summary>
    /// <remarks>Access: MANAGER, ADMIN</remarks>
    [HttpPost("bulk")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> BulkCreate([FromBody] object request)
        => Ok("Bulk create endpoint pending service implementation");

    /// <summary>Update parking spot</summary>
    /// <remarks>Access: MANAGER, ADMIN</remarks>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] object request)
        => Ok("Update endpoint pending service implementation");

    /// <summary>Mark spot as occupied</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN</remarks>
    [HttpPut("{id:int}/occupy")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Occupy(int id)
        => Ok("Occupy endpoint pending service implementation");

    /// <summary>Release occupied spot</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN</remarks>
    [HttpPut("{id:int}/release")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Release(int id)
        => Ok("Release endpoint pending service implementation");

    /// <summary>Delete parking spot permanently</summary>
    /// <remarks>Access: ADMIN only</remarks>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
        => Ok("Delete endpoint pending service implementation");

    /// <summary>Get total spot count for parking lot</summary>
    /// <remarks>Access: MANAGER, ADMIN</remarks>
    [HttpGet("lot/{lotId:int}/count")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Count(int lotId)
        => Ok("Count endpoint pending service implementation");
}