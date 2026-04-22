namespace Booking.Domain.ValueObjects;

public class BookingTimeWindow
{
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public bool IsValid()
    {
        return EndTimeUtc > StartTimeUtc;
    }
}