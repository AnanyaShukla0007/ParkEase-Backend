namespace Booking.Application.DTOs.Responses;

public class BookingListResponse
{
    public List<BookingResponse> Items { get; set; } = [];
    public int Count { get; set; }
}