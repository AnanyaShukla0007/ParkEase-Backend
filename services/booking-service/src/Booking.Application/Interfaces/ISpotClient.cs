namespace Booking.Application.Interfaces;

public interface ISpotClient
{
    Task<bool> ReserveSpotAsync(int spotId);
    Task<bool> OccupySpotAsync(int spotId);
    Task<bool> ReleaseSpotAsync(int spotId);
}