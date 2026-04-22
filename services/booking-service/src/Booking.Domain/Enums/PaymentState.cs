namespace Booking.Domain.Enums;

public enum PaymentState
{
    NotRequired = 1,
    Pending = 2,
    Paid = 3,
    Failed = 4,
    Refunded = 5
}