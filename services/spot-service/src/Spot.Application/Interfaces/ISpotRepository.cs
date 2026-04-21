using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;

namespace Spot.Application.Interfaces;

public interface ISpotRepository
{
    Task<List<SpotEntity>> GetAllAsync();
    Task AddAsync(SpotEntity spot);
}