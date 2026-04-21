using ParkEase.Spot.Application.DTOs;

namespace Spot.Application.Interfaces;

public interface ISpotService
{
    Task<List<SpotResponse>> GetAllAsync();
    Task<SpotResponse?> GetByIdAsync(int id);
    Task<List<SpotResponse>> GetByLotAsync(int lotId);
    Task<List<SpotResponse>> GetAvailableByLotAsync(int lotId);

    Task<SpotResponse> CreateAsync(CreateSpotRequest request);
}