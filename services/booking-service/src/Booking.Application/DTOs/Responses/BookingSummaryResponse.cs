namespace Booking.Application.DTOs.Responses;

public class BookingSummaryResponse
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Completed { get; set; }
    public int Cancelled { get; set; }
}