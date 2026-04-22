using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Requests;

public class CreateBookingRequest
{
    public int UserId { get; set; }
    public int LotId { get; set; }
    public int SpotId { get; set; }
    public int VehicleId { get; set; }

    public string VehiclePlate { get; set; } = string.Empty;

    public BookingType BookingType { get; set; }

    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public decimal EstimatedAmount { get; set; }
    public string Notes { get; set; } = string.Empty;
}