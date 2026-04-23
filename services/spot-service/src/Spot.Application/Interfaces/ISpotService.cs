using ParkEase.Spot.Application.DTOs;

namespace Spot.Application.Interfaces;

public interface ISpotService
{
    Task<List<SpotResponse>> GetAllAsync();
    Task<SpotResponse?> GetByIdAsync(int id);
    Task<List<SpotResponse>> GetByLotAsync(int lotId);
    Task<List<SpotResponse>> GetAvailableByLotAsync(int lotId);
    Task<SpotResponse> CreateAsync(CreateSpotRequest request);
    Task<List<SpotResponse>> BulkCreateAsync(BulkCreateSpotRequest request);
    Task<SpotResponse?> UpdateAsync(int id, UpdateSpotRequest request);
    Task<bool> DeleteAsync(int id);

    Task<SpotResponse?> ReserveAsync(int id);
    Task<SpotResponse?> OccupyAsync(int id);
    Task<SpotResponse?> ReleaseAsync(int id);

    Task<object> GetCountAsync(int lotId);
}
