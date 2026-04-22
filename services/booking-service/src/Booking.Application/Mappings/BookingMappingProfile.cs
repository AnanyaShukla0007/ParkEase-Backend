using Booking.Application.DTOs.Responses;
using BookingEntity = Booking.Domain.Entities.Booking;

namespace Booking.Application.Mappings;

public static class BookingMappingProfile
{
    public static BookingResponse ToResponse(BookingEntity booking)
    {
        return new BookingResponse
        {
            Id = booking.Id,
            UserId = booking.UserId,
            LotId = booking.LotId,
            SpotId = booking.SpotId,
            VehicleId = booking.VehicleId,
            VehiclePlate = booking.VehiclePlate,
            BookingType = booking.BookingType,
            Status = booking.Status,
            PaymentState = booking.PaymentState,
            StartTimeUtc = booking.StartTimeUtc,
            EndTimeUtc = booking.EndTimeUtc,
            CheckInTimeUtc = booking.CheckInTimeUtc,
            CheckOutTimeUtc = booking.CheckOutTimeUtc,
            EstimatedAmount = booking.EstimatedAmount,
            FinalAmount = booking.FinalAmount,
            CreatedAtUtc = booking.CreatedAtUtc
        };
    }
}