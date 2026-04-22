namespace Booking.Application.DTOs.Responses;

public class SpotLookupResponse
{
    public int SpotId { get; set; }
    public int LotId { get; set; }
    public string SpotNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
}
