using Booking.Domain.Enums;

namespace Booking.Domain.Entities;

public class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int LotId { get; set; }
    public int SpotId { get; set; }
    public int VehicleId { get; set; }

    public string VehiclePlate { get; set; } = string.Empty;

    public BookingType BookingType { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public PaymentState PaymentState { get; set; } = PaymentState.NotRequired;

    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public DateTime? CheckInTimeUtc { get; set; }
    public DateTime? CheckOutTimeUtc { get; set; }

    public decimal EstimatedAmount { get; set; }
    public decimal FinalAmount { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}