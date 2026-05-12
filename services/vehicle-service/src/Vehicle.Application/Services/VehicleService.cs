using Vehicle.Application.DTOs;
using Vehicle.Application.Interfaces;
using Vehicle.Application.Validators;
using Vehicle.Domain.Enums;
using VehicleEntity = Vehicle.Domain.Entities.Vehicle;

namespace Vehicle.Application.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<VehicleResponse> RegisterVehicleAsync(CreateVehicleRequest request)
    {
        var errors = CreateVehicleValidator.Validate(request);

        if (errors.Count > 0)
            throw new InvalidOperationException(string.Join(" | ", errors));

        var normalizedPlate = NormalizePlate(request.LicensePlate);
        var exists = await _vehicleRepository.ExistsByLicensePlateAsync(request.OwnerId, normalizedPlate);

        if (exists)
            throw new InvalidOperationException("License plate already exists for this owner.");

        var vehicle = new VehicleEntity
        {
            OwnerId = request.OwnerId,
            LicensePlate = normalizedPlate,
            Make = request.Make.Trim(),
            Model = request.Model.Trim(),
            Color = request.Color.Trim(),
            VehicleType = request.VehicleType,
            IsEV = request.IsEV,
            IsActive = request.IsActive,
            RegisteredAt = DateTime.UtcNow
        };

        var saved = await _vehicleRepository.AddAsync(vehicle);
        return Map(saved);
    }

    public async Task<VehicleResponse?> GetVehicleByIdAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepository.FindByVehicleIdAsync(vehicleId);
        return vehicle is null ? null : Map(vehicle);
    }

    public async Task<List<VehicleResponse>> GetVehiclesByOwnerAsync(int ownerId)
        => (await _vehicleRepository.FindByOwnerIdAsync(ownerId)).Select(Map).ToList();

    public async Task<VehicleResponse?> GetByLicensePlateAsync(string licensePlate)
    {
        var vehicle = await _vehicleRepository.FindByLicensePlateAsync(NormalizePlate(licensePlate));
        return vehicle is null ? null : Map(vehicle);
    }

    public async Task<List<VehicleResponse>> GetByVehicleTypeAsync(VehicleType vehicleType)
        => (await _vehicleRepository.FindByVehicleTypeAsync(vehicleType)).Select(Map).ToList();

    public async Task<List<VehicleResponse>> GetByIsEvAsync(bool isEv)
        => (await _vehicleRepository.FindByIsEvAsync(isEv)).Select(Map).ToList();

    public async Task<List<VehicleResponse>> GetAllVehiclesAsync()
        => (await _vehicleRepository.GetAllAsync()).Select(Map).ToList();

    public async Task<VehicleResponse> UpdateVehicleAsync(int vehicleId, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.FindByVehicleIdAsync(vehicleId)
            ?? throw new KeyNotFoundException("Vehicle not found.");

        if (!string.IsNullOrWhiteSpace(request.LicensePlate))
        {
            var normalizedPlate = NormalizePlate(request.LicensePlate);
            var exists = await _vehicleRepository.ExistsByLicensePlateAsync(vehicle.OwnerId, normalizedPlate, vehicleId);

            if (exists)
                throw new InvalidOperationException("License plate already exists for this owner.");

            vehicle.LicensePlate = normalizedPlate;
        }

        if (!string.IsNullOrWhiteSpace(request.Make))
            vehicle.Make = request.Make.Trim();

        if (!string.IsNullOrWhiteSpace(request.Model))
            vehicle.Model = request.Model.Trim();

        if (!string.IsNullOrWhiteSpace(request.Color))
            vehicle.Color = request.Color.Trim();

        if (request.VehicleType.HasValue)
            vehicle.VehicleType = request.VehicleType.Value;

        if (request.IsEV.HasValue)
            vehicle.IsEV = request.IsEV.Value;

        if (request.IsActive.HasValue)
            vehicle.IsActive = request.IsActive.Value;

        await _vehicleRepository.UpdateAsync(vehicle);
        return Map(vehicle);
    }

    public async Task DeleteVehicleAsync(int vehicleId)
    {
        var vehicle = await _vehicleRepository.FindByVehicleIdAsync(vehicleId)
            ?? throw new KeyNotFoundException("Vehicle not found.");

        await _vehicleRepository.DeleteByVehicleIdAsync(vehicle);
    }

    private static string NormalizePlate(string plate)
        => plate.Trim().ToUpperInvariant();

    private static VehicleResponse Map(VehicleEntity vehicle) => new()
    {
        VehicleId = vehicle.VehicleId,
        OwnerId = vehicle.OwnerId,
        LicensePlate = vehicle.LicensePlate,
        Make = vehicle.Make,
        Model = vehicle.Model,
        Color = vehicle.Color,
        VehicleType = vehicle.VehicleType,
        IsEV = vehicle.IsEV,
        RegisteredAt = vehicle.RegisteredAt,
        IsActive = vehicle.IsActive
    };
}
