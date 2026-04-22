using Vehicle.Domain.Entities;
using Vehicle.Domain.Enums;

namespace Vehicle.Application.Interfaces;

public interface IVehicleRepository
{
    Task<Domain.Entities.Vehicle> AddAsync(Domain.Entities.Vehicle vehicle);
    Task<Domain.Entities.Vehicle?> FindByVehicleIdAsync(int vehicleId);
    Task<List<Domain.Entities.Vehicle>> FindByOwnerIdAsync(int ownerId);
    Task<Domain.Entities.Vehicle?> FindByLicensePlateAsync(string licensePlate);
    Task<List<Domain.Entities.Vehicle>> FindByVehicleTypeAsync(VehicleType vehicleType);
    Task<List<Domain.Entities.Vehicle>> FindByIsEvAsync(bool isEv);
    Task<List<Domain.Entities.Vehicle>> GetAllAsync();
    Task<bool> ExistsByLicensePlateAsync(int ownerId, string licensePlate, int? excludeVehicleId = null);
    Task UpdateAsync(Domain.Entities.Vehicle vehicle);
    Task DeleteByVehicleIdAsync(Domain.Entities.Vehicle vehicle);
}
