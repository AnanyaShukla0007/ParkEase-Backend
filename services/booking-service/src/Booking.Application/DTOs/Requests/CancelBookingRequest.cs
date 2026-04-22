namespace Booking.Application.DTOs.Requests;

public class CancelBookingRequest
{
    public string Reason { get; set; } = string.Empty;
}