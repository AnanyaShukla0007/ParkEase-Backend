using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;

namespace Spot.Application.Interfaces;

public interface ISpotRepository
{
    Task<List<SpotEntity>> GetAllAsync();

    Task<SpotEntity?> GetByIdAsync(int id);

    Task<List<SpotEntity>> GetByLotAsync(int lotId);

    Task<List<SpotEntity>> GetAvailableByLotAsync(int lotId);

    Task AddAsync(SpotEntity entity);

    Task UpdateAsync(SpotEntity entity);

    Task DeleteAsync(SpotEntity entity);
}