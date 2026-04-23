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
    /// <remarks>Access: Public (Guest, Driver, Manager, Admin). Returns complete list of spots.</remarks>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>Get parking spot by ID</summary>
    /// <remarks>Access: Public. Returns details of a single parking spot.</remarks>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Spot not found."
            });
        }

        return Ok(result);
    }

    /// <summary>Get all spots by parking lot</summary>
    /// <remarks>Access: Public. Returns every spot mapped to the selected parking lot.</remarks>
    [HttpGet("lot/{lotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByLot(int lotId)
    {
        return Ok(await _service.GetByLotAsync(lotId));
    }

    /// <summary>Get available spots by parking lot</summary>
    /// <remarks>Access: Public. Returns only spots currently available for booking.</remarks>
    [HttpGet("lot/{lotId:int}/available")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAvailable(int lotId)
    {
        return Ok(await _service.GetAvailableByLotAsync(lotId));
    }

    /// <summary>Create new parking spot</summary>
    /// <remarks>Access: MANAGER, ADMIN. Creates a new parking spot under the selected parking lot.</remarks>
    [HttpPost]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreateSpotRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    /// <summary>Reserve available spot</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Marks the selected spot as Reserved for a booking.</remarks>
    [HttpPut("{id:int}/reserve")]
    [AllowAnonymous]
    public async Task<IActionResult> Reserve(int id)
    {
        var result = await _service.ReserveAsync(id);

        if (result == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to reserve spot."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Spot reserved successfully.",
            spotId = result.SpotId,
            status = result.Status.ToString()
        });
    }

    /// <summary>Mark spot as occupied</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Changes spot status to Occupied and returns updated details.</remarks>
    [HttpPut("{id:int}/occupy")]
    [AllowAnonymous]
    public async Task<IActionResult> Occupy(int id)
    {
        var result = await _service.OccupyAsync(id);

        if (result == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to occupy spot."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Spot occupied successfully.",
            spotId = result.SpotId,
            status = result.Status.ToString()
        });
    }

    /// <summary>Release occupied spot</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Changes spot status back to Available and returns updated details.</remarks>
    [HttpPut("{id:int}/release")]
    [AllowAnonymous]
    public async Task<IActionResult> Release(int id)
    {
        var result = await _service.ReleaseAsync(id);

        if (result == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Unable to release spot."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Spot released successfully.",
            spotId = result.SpotId,
            status = result.Status.ToString()
        });
    }

    /// <summary>Get total spot statistics for parking lot</summary>
    /// <remarks>Access: MANAGER, ADMIN. Returns total, available, and occupied spot counts.</remarks>
    [HttpGet("lot/{lotId:int}/count")]
    [AllowAnonymous]
    public async Task<IActionResult> Count(int lotId)
    {
        var result = await _service.GetCountAsync(lotId);
        return Ok(result);
    }

    /// <summary>Bulk create parking spots</summary>
    /// <remarks>Access: MANAGER, ADMIN. Creates multiple parking spots for a lot in one request.</remarks>
    [HttpPost("bulk")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> BulkCreate([FromBody] BulkCreateSpotRequest request)
    {
        var result = await _service.BulkCreateAsync(request);

        return Ok(new
        {
            success = true,
            message = "Spots created successfully.",
            data = result
        });
    }

    /// <summary>Update parking spot</summary>
    /// <remarks>Access: MANAGER, ADMIN. Updates an existing parking spot.</remarks>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSpotRequest request)
    {
        var result = await _service.UpdateAsync(id, request);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Spot not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Spot updated successfully.",
            data = result
        });
    }

    /// <summary>Delete parking spot permanently</summary>
    /// <remarks>Access: ADMIN only. Deletes a parking spot permanently.</remarks>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);

        if (!ok)
        {
            return NotFound(new
            {
                success = false,
                message = "Spot not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Spot deleted successfully."
        });
    }
}
