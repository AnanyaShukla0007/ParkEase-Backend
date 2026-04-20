using Microsoft.EntityFrameworkCore;
using ParkingLot.Application.Interfaces;
using ParkingLot.Infrastructure.Persistence;

namespace ParkingLot.Infrastructure.Repositories;

public class ParkingLotRepository : IParkingLotRepository
{
    private readonly ParkingLotDbContext _context;

    public ParkingLotRepository(ParkingLotDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.ParkingLot> AddAsync(Domain.Entities.ParkingLot lot)
    {
        _context.ParkingLots.Add(lot);
        await _context.SaveChangesAsync();
        return lot;
    }

    public async Task<Domain.Entities.ParkingLot?> GetByIdAsync(int id)
    {
        return await _context.ParkingLots.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Domain.Entities.ParkingLot>> GetAllAsync()
    {
        return await _context.ParkingLots.ToListAsync();
    }

    public async Task<List<Domain.Entities.ParkingLot>> GetByCityAsync(string city)
    {
        return await _context.ParkingLots
            .Where(x => x.City.ToLower() == city.ToLower())
            .ToListAsync();
    }

    public async Task<List<Domain.Entities.ParkingLot>> GetByManagerAsync(int managerId)
    {
        return await _context.ParkingLots
            .Where(x => x.ManagerId == managerId)
            .ToListAsync();
    }

    public async Task UpdateAsync(Domain.Entities.ParkingLot lot)
    {
        _context.ParkingLots.Update(lot);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Domain.Entities.ParkingLot lot)
    {
        _context.ParkingLots.Remove(lot);
        await _context.SaveChangesAsync();
    }
}