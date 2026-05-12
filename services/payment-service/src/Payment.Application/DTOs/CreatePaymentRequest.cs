using Payment.Domain.Enums;

namespace Payment.Application.DTOs;

public class CreatePaymentRequest
{
    public int BookingId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public PaymentMethod PaymentMethod { get; set; }
    public string? Notes { get; set; }
}
