using Microsoft.EntityFrameworkCore;
using Vehicle.Application.Interfaces;
using Vehicle.Domain.Enums;
using Vehicle.Infrastructure.Persistence;
using VehicleEntity = Vehicle.Domain.Entities.Vehicle;

namespace Vehicle.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly VehicleDbContext _context;

    public VehicleRepository(VehicleDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleEntity> AddAsync(VehicleEntity vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<VehicleEntity?> FindByVehicleIdAsync(int vehicleId)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);
    }

    public async Task<List<VehicleEntity>> FindByOwnerIdAsync(int ownerId)
    {
        return await _context.Vehicles
            .Where(x => x.OwnerId == ownerId)
            .OrderByDescending(x => x.RegisteredAt)
            .ToListAsync();
    }

    public async Task<VehicleEntity?> FindByLicensePlateAsync(string licensePlate)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(x => x.LicensePlate == licensePlate);
    }

    public async Task<List<VehicleEntity>> FindByVehicleTypeAsync(VehicleType vehicleType)
    {
        return await _context.Vehicles
            .Where(x => x.VehicleType == vehicleType)
            .OrderByDescending(x => x.RegisteredAt)
            .ToListAsync();
    }

    public async Task<List<VehicleEntity>> FindByIsEvAsync(bool isEv)
    {
        return await _context.Vehicles
            .Where(x => x.IsEV == isEv)
            .OrderByDescending(x => x.RegisteredAt)
            .ToListAsync();
    }

    public async Task<List<VehicleEntity>> GetAllAsync()
    {
        return await _context.Vehicles
            .OrderByDescending(x => x.RegisteredAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByLicensePlateAsync(int ownerId, string licensePlate, int? excludeVehicleId = null)
    {
        return await _context.Vehicles.AnyAsync(x =>
            x.OwnerId == ownerId &&
            x.LicensePlate == licensePlate &&
            (!excludeVehicleId.HasValue || x.VehicleId != excludeVehicleId.Value));
    }

    public async Task UpdateAsync(VehicleEntity vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByVehicleIdAsync(VehicleEntity vehicle)
    {
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
    }
}
