using Booking.Domain.Enums;

namespace Booking.Application.DTOs.Responses;

public class BookingResponse
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int LotId { get; set; }
    public int SpotId { get; set; }
    public int VehicleId { get; set; }

    public string VehiclePlate { get; set; } = string.Empty;

    public BookingType BookingType { get; set; }
    public BookingStatus Status { get; set; }
    public PaymentState PaymentState { get; set; }

    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }

    public DateTime? CheckInTimeUtc { get; set; }
    public DateTime? CheckOutTimeUtc { get; set; }

    public decimal EstimatedAmount { get; set; }
    public decimal FinalAmount { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}