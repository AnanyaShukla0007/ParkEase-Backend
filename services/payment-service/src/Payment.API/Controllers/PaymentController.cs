using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.DTOs;
using Payment.Application.Interfaces;

namespace Payment.API.Controllers;

[ApiController]
[Route("api/v1/payments")]
[Produces("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>Create a new payment record</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Creates a pending payment linked to a booking before it is processed.</remarks>
    [HttpPost]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        var result = await _paymentService.CreateAsync(request);

        return Ok(new
        {
            success = true,
            message = "Payment created successfully.",
            data = result
        });
    }

    /// <summary>Get payment by ID</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Returns complete payment details for the selected payment.</remarks>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _paymentService.GetByIdAsync(id);

        if (result is null)
        {
            return NotFound(new
            {
                success = false,
                message = "Payment not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    /// <summary>Get payments by booking</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Returns all payments created for the selected booking.</remarks>
    [HttpGet("booking/{bookingId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByBooking(int bookingId)
    {
        return Ok(new
        {
            success = true,
            data = await _paymentService.GetByBookingIdAsync(bookingId)
        });
    }

    /// <summary>Get payments by user</summary>
    /// <remarks>Access: DRIVER, ADMIN. Returns payment history for the selected user.</remarks>
    [HttpGet("user/{userId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return Ok(new
        {
            success = true,
            data = await _paymentService.GetByUserIdAsync(userId)
        });
    }

    /// <summary>Get all payments</summary>
    /// <remarks>Access: ADMIN. Returns the full payment ledger across all bookings.</remarks>
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok(new
        {
            success = true,
            data = await _paymentService.GetAllAsync()
        });
    }

    /// <summary>Process payment</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Marks a pending payment as paid and stores provider references.</remarks>
    [HttpPut("{id:int}/process")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Process(int id, [FromBody] ProcessPaymentRequest request)
    {
        var result = await _paymentService.ProcessAsync(id, request);

        return Ok(new
        {
            success = true,
            message = "Payment processed successfully.",
            data = result
        });
    }

    /// <summary>Mark payment as failed</summary>
    /// <remarks>Access: DRIVER, MANAGER, ADMIN. Marks a payment attempt as failed with an optional failure reason.</remarks>
    [HttpPut("{id:int}/fail")]
    [Authorize(Roles = "DRIVER,MANAGER,ADMIN")]
    public async Task<IActionResult> Fail(int id, [FromBody] FailPaymentRequest request)
    {
        var result = await _paymentService.FailAsync(id, request);

        return Ok(new
        {
            success = true,
            message = "Payment marked as failed.",
            data = result
        });
    }

    /// <summary>Refund payment</summary>
    /// <remarks>Access: MANAGER, ADMIN. Refunds a previously paid payment and stores the refund reason.</remarks>
    [HttpPut("{id:int}/refund")]
    [Authorize(Roles = "MANAGER,ADMIN")]
    public async Task<IActionResult> Refund(int id, [FromBody] RefundPaymentRequest request)
    {
        var result = await _paymentService.RefundAsync(id, request);

        return Ok(new
        {
            success = true,
            message = "Payment refunded successfully.",
            data = result
        });
    }
}
