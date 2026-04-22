using Microsoft.EntityFrameworkCore;
using Spot.Application.Interfaces;
using ParkEase.Spot.Infrastructure.Persistence;
using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;
using ParkEase.Spot.Domain.Enums;

namespace ParkEase.Spot.Infrastructure.Repositories;

public class SpotRepository : ISpotRepository
{
    private readonly SpotDbContext _context;

    public SpotRepository(SpotDbContext context)
    {
        _context = context;
    }

    public async Task<List<SpotEntity>> GetAllAsync()
    {
        return await _context.Spots.ToListAsync();
    }

    public async Task<SpotEntity?> GetByIdAsync(int id)
    {
        return await _context.Spots
            .FirstOrDefaultAsync(x => x.SpotId == id);
    }

    public async Task<List<SpotEntity>> GetByLotAsync(int lotId)
    {
        return await _context.Spots
            .Where(x => x.LotId == lotId)
            .ToListAsync();
    }

    public async Task<List<SpotEntity>> GetAvailableByLotAsync(int lotId)
    {
        return await _context.Spots
            .Where(x => x.LotId == lotId &&
                        x.Status == SpotStatus.Available)
            .ToListAsync();
    }

    public async Task AddAsync(SpotEntity entity)
    {
        await _context.Spots.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SpotEntity entity)
    {
        _context.Spots.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(SpotEntity entity)
    {
        _context.Spots.Remove(entity);
        await _context.SaveChangesAsync();
    }
}