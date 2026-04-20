using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.Application.DTOs;
using ParkingLot.Application.Interfaces;

namespace ParkingLot.API.Controllers;

[ApiController]
[Route("api/v1/parkinglots")]
[Produces("application/json")]
public class ParkingLotsController : ControllerBase
{
    private readonly IParkingLotService _service;

    public ParkingLotsController(IParkingLotService service)
    {
        _service = service;
    }

    /// <summary>Create a new parking lot</summary>
    [HttpPost]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreateParkingLotRequest request)
        => Ok(await _service.CreateAsync(request));

    /// <summary>Get all parking lots</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    /// <summary>Get parking lot by ID</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _service.GetByIdAsync(id));

    /// <summary>Get parking lots by city</summary>
    [HttpGet("city/{city}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCity(string city)
        => Ok(await _service.GetByCityAsync(city));

    /// <summary>Get parking lots by manager</summary>
    [HttpGet("manager/{managerId:int}")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> GetByManager(int managerId)
        => Ok(await _service.GetByManagerAsync(managerId));

    /// <summary>Find nearby parking lots</summary>
    [HttpGet("nearby")]
    [AllowAnonymous]
    public async Task<IActionResult> Nearby([FromQuery] double lat, [FromQuery] double lng)
        => Ok(await _service.GetNearbyAsync(lat, lng));

    /// <summary>Update parking lot</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateParkingLotRequest request)
        => Ok(await _service.UpdateAsync(id, request));

    /// <summary>Approve parking lot</summary>
    [HttpPatch("{id:int}/approve")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Approve(int id)
    {
        await _service.ToggleApprovalAsync(id, true);
        return Ok(new { message = "Parking lot approved." });
    }

    /// <summary>Reject parking lot</summary>
    [HttpPatch("{id:int}/reject")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Reject(int id)
    {
        await _service.ToggleApprovalAsync(id, false);
        return Ok(new { message = "Parking lot rejected." });
    }

    /// <summary>Activate parking lot</summary>
    [HttpPatch("{id:int}/activate")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Activate(int id)
    {
        await _service.ToggleActiveAsync(id, true);
        return Ok(new { message = "Parking lot activated." });
    }

    /// <summary>Deactivate parking lot</summary>
    [HttpPatch("{id:int}/deactivate")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _service.ToggleActiveAsync(id, false);
        return Ok(new { message = "Parking lot deactivated." });
    }

    /// <summary>Delete parking lot permanently</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok(new { message = "Parking lot deleted." });
    }
}