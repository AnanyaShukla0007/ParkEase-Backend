using Vehicle.Application.DTOs;
using Vehicle.Domain.Enums;

namespace Vehicle.Application.Interfaces;

public interface IVehicleService
{
    Task<VehicleResponse> RegisterVehicleAsync(CreateVehicleRequest request);
    Task<VehicleResponse?> GetVehicleByIdAsync(int vehicleId);
    Task<List<VehicleResponse>> GetVehiclesByOwnerAsync(int ownerId);
    Task<VehicleResponse?> GetByLicensePlateAsync(string licensePlate);
    Task<List<VehicleResponse>> GetByVehicleTypeAsync(VehicleType vehicleType);
    Task<List<VehicleResponse>> GetByIsEvAsync(bool isEv);
    Task<List<VehicleResponse>> GetAllVehiclesAsync();
    Task<VehicleResponse> UpdateVehicleAsync(int vehicleId, UpdateVehicleRequest request);
    Task DeleteVehicleAsync(int vehicleId);
}
