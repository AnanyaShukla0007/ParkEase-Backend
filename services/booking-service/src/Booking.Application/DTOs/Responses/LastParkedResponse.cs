namespace Booking.Application.DTOs.Responses;

public class LastParkedResponse
{
    public int BookingId { get; set; }
    public int LotId { get; set; }
    public int SpotId { get; set; }
    public string LotName { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string? Section { get; set; }
    public string SpotNumber { get; set; } = string.Empty;
    public DateTime ParkedAtUtc { get; set; }
    public string? Note { get; set; }
    public string? PhotoUrl { get; set; }
}
