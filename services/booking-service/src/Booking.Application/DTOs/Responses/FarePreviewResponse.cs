namespace Booking.Application.DTOs.Responses;

public class FarePreviewResponse
{
    public int BookingId { get; set; }
    public decimal EstimatedAmount { get; set; }
    public decimal FinalAmount { get; set; }
}