using Booking.Application.DTOs.Requests;
using Booking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers;

[ApiController]
[Route("api/v1/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    /// <summary>
    /// Create a new booking.
    /// </summary>
    /// <remarks>
    /// Role: Driver
    /// Reserves selected spot and creates a booking record.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        var result = await _bookingService.CreateAsync(request);
        return Ok(new
        {
            success = true,
            message = "Booking created successfully.",
            data = result
        });
    }

    /// <summary>
    /// Get booking by ID.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Manager / Admin
    /// Returns complete booking details.
    /// </remarks>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _bookingService.GetByIdAsync(id);

        if (result is null)
            return NotFound(new
            {
                success = false,
                message = "Booking not found."
            });

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    /// <summary>
    /// Get bookings by user ID.
    /// </summary>
    /// <remarks>
    /// Role: Driver (self), Admin
    /// Used for booking history and active bookings.
    /// </remarks>
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return Ok(new
        {
            success = true,
            data = await _bookingService.GetByUserIdAsync(userId)
        });
    }

    /// <summary>
    /// Get the user's last parked location.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Admin
    /// Returns the driver's latest checked-in parking memory for My Parking / Last Parked UI.
    /// </remarks>
    [HttpGet("user/{userId:int}/last-parked")]
    public async Task<IActionResult> GetLastParked(int userId)
    {
        var result = await _bookingService.GetLastParkedAsync(userId);

        if (result is null)
        {
            return NotFound(new
            {
                success = false,
                message = "No parked history found for this user."
            });
        }

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    /// <summary>
    /// Get bookings by parking lot ID.
    /// </summary>
    /// <remarks>
    /// Role: Manager / Admin
    /// Operational view for one parking lot.
    /// </remarks>
    [HttpGet("lot/{lotId:int}")]
    public async Task<IActionResult> GetByLot(int lotId)
    {
        return Ok(new
        {
            success = true,
            data = await _bookingService.GetByLotIdAsync(lotId)
        });
    }

    /// <summary>
    /// Get all bookings.
    /// </summary>
    /// <remarks>
    /// Role: Admin
    /// Full system booking audit.
    /// </remarks>
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(new
        {
            success = true,
            data = await _bookingService.GetAllAsync()
        });
    }

    /// <summary>
    /// Cancel an existing booking.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Manager / Admin
    /// Releases reserved spot and restores lot availability.
    /// </remarks>
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancelBookingRequest request)
    {
        var ok = await _bookingService.CancelAsync(id, request);

        if (!ok)
            return BadRequest(new
            {
                success = false,
                message = "Unable to cancel booking."
            });

        return Ok(new
        {
            success = true,
            message = "Booking cancelled successfully.",
            bookingId = id
        });
    }

    /// <summary>
    /// Check in vehicle.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Gate Staff / Manager
    /// Changes booking from Confirmed to Active.
    /// </remarks>
    [HttpPut("{id:int}/checkin")]
    public async Task<IActionResult> CheckIn(int id)
    {
        var ok = await _bookingService.CheckInAsync(id);

        if (!ok)
            return BadRequest(new
            {
                success = false,
                message = "Unable to check in booking."
            });

        return Ok(new
        {
            success = true,
            message = "Checked in successfully.",
            bookingId = id
        });
    }

    /// <summary>
    /// Check out vehicle.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Gate Staff / Manager
    /// Completes booking and frees spot.
    /// </remarks>
    [HttpPut("{id:int}/checkout")]
    public async Task<IActionResult> CheckOut(int id, CheckOutRequest request)
    {
        var ok = await _bookingService.CheckOutAsync(id, request);

        if (!ok)
            return BadRequest(new
            {
                success = false,
                message = "Unable to check out booking."
            });

        return Ok(new
        {
            success = true,
            message = "Checked out successfully.",
            bookingId = id
        });
    }

    /// <summary>
    /// Extend booking time.
    /// </summary>
    /// <remarks>
    /// Role: Driver
    /// Updates booking end time.
    /// </remarks>
    [HttpPut("{id:int}/extend")]
    public async Task<IActionResult> Extend(int id, ExtendBookingRequest request)
    {
        var ok = await _bookingService.ExtendAsync(id, request);

        if (!ok)
            return BadRequest(new
            {
                success = false,
                message = "Unable to extend booking."
            });

        return Ok(new
        {
            success = true,
            message = "Booking extended successfully.",
            bookingId = id
        });
    }

    /// <summary>
    /// Preview payable fare.
    /// </summary>
    /// <remarks>
    /// Role: Driver / Manager
    /// Calculates estimated final amount without checkout.
    /// </remarks>
    [HttpGet("{id:int}/fare-preview")]
    public async Task<IActionResult> FarePreview(int id)
    {
        var result = await _bookingService.GetFarePreviewAsync(id);

        if (result is null)
            return NotFound(new
            {
                success = false,
                message = "Booking not found."
            });

        return Ok(new
        {
            success = true,
            data = result
        });
    }
}
