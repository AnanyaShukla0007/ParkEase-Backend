using Booking.Application.DTOs.Responses;

namespace Booking.Application.Interfaces;

public interface IParkingLotClient
{
    Task<bool> DecrementAvailableAsync(int lotId);
    Task<bool> IncrementAvailableAsync(int lotId);
    Task<ParkingLotLookupResponse?> GetParkingLotByIdAsync(int lotId);
}
