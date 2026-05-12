namespace Booking.Application.DTOs.Requests;

public class SearchBookingRequest
{
    public int? UserId { get; set; }
    public int? LotId { get; set; }
    public int? Status { get; set; }

    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }
}