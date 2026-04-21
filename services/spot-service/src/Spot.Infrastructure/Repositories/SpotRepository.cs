using Microsoft.EntityFrameworkCore;
using Spot.Application.Interfaces;
using ParkEase.Spot.Infrastructure.Persistence;
using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;

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
        return await _context.Spots
            .OrderBy(x => x.Floor)
            .ThenBy(x => x.SpotNumber)
            .ToListAsync();
    }

    public async Task AddAsync(SpotEntity spot)
    {
        await _context.Spots.AddAsync(spot);
        await _context.SaveChangesAsync();
    }
}