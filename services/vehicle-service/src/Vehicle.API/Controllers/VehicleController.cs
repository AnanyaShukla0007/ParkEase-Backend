using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vehicle.Application.DTOs;
using Vehicle.Application.Interfaces;
using Vehicle.Domain.Enums;

namespace Vehicle.API.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    /// <summary>Register a vehicle</summary>
    /// <remarks>Access: DRIVER, ADMIN. Adds a vehicle with plate number, make, model, color, type, EV flag, and active status.</remarks>
    [HttpPost]
    [Authorize(Roles = "DRIVER,ADMIN")]
    public async Task<IActionResult> Register([FromBody] CreateVehicleRequest request)
    {
        var result = await _vehicleService.RegisterVehicleAsync(request);

        return Ok(new
        {
            success = true,
            message = "Vehicle registered successfully.",
            data = result
        });
    }

    /// <summary>Get vehicle by ID</summary>
    /// <remarks>Access: DRIVER, ADMIN. Returns complete vehicle details for the selected vehicle.</remarks>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _vehicleService.GetVehicleByIdAsync(id);

        if (result is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Vehicle not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    /// <summary>Get vehicles by owner</summary>
    /// <remarks>Access: DRIVER, ADMIN. Lists all vehicles registered to the selected owner account.</remarks>
    [HttpGet("owner/{ownerId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByOwner(int ownerId)
    {
        return Ok(new
        {
            success = true,
            data = await _vehicleService.GetVehiclesByOwnerAsync(ownerId)
        });
    }

    /// <summary>Get vehicle by license plate</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Finds a vehicle using its license plate.</remarks>
    [HttpGet("plate/{licensePlate}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByLicensePlate(string licensePlate)
    {
        var result = await _vehicleService.GetByLicensePlateAsync(licensePlate);

        if (result is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Vehicle not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    /// <summary>Get all vehicles</summary>
    /// <remarks>Access: ADMIN. Returns all vehicles registered on the platform.</remarks>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok(new
        {
            success = true,
            data = await _vehicleService.GetAllVehiclesAsync()
        });
    }

    /// <summary>Get vehicles by type</summary>
    /// <remarks>Access: DRIVER, ADMIN. Returns vehicles filtered by vehicle type such as 2W, 4W, or HEAVY.</remarks>
    [HttpGet("type/{vehicleType}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByType(VehicleType vehicleType)
    {
        return Ok(new
        {
            success = true,
            data = await _vehicleService.GetByVehicleTypeAsync(vehicleType)
        });
    }

    /// <summary>Get EV vehicles</summary>
    /// <remarks>Access: DRIVER, ADMIN. Returns vehicles filtered by EV status.</remarks>
    [HttpGet("is-ev/{isEv:bool}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEvStatus(bool isEv)
    {
        return Ok(new
        {
            success = true,
            data = await _vehicleService.GetByIsEvAsync(isEv)
        });
    }

    /// <summary>Update vehicle</summary>
    /// <remarks>Access: DRIVER, ADMIN. Updates the selected vehicle's plate, make, model, color, type, EV flag, and active status.</remarks>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "DRIVER,ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehicleRequest request)
    {
        var result = await _vehicleService.UpdateVehicleAsync(id, request);

        return Ok(new
        {
            success = true,
            message = "Vehicle updated successfully.",
            data = result
        });
    }

    /// <summary>Delete vehicle</summary>
    /// <remarks>Access: DRIVER, ADMIN. Deletes the selected vehicle from the owner's account.</remarks>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "DRIVER,ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        await _vehicleService.DeleteVehicleAsync(id);

        return Ok(new
        {
            success = true,
            message = "Vehicle deleted successfully."
        });
    }
}
