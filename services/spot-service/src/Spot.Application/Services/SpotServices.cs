using ParkEase.Spot.Application.DTOs;
using Spot.Application.Interfaces;
using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;
using ParkEase.Spot.Domain.Enums;

namespace Spot.Application.Services;

public class SpotServices : ISpotService
{
    private readonly ISpotRepository _repository;

    public SpotServices(ISpotRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SpotResponse>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(Map).ToList();

    public async Task<SpotResponse?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : Map(entity);
    }

    public async Task<List<SpotResponse>> GetByLotAsync(int lotId)
        => (await _repository.GetByLotAsync(lotId)).Select(Map).ToList();

    public async Task<List<SpotResponse>> GetAvailableByLotAsync(int lotId)
        => (await _repository.GetAvailableByLotAsync(lotId)).Select(Map).ToList();

    public async Task<SpotResponse> CreateAsync(CreateSpotRequest request)
    {
        var entity = new SpotEntity
        {
            LotId = request.LotId,
            SpotNumber = request.SpotNumber,
            Floor = request.Floor,
            SpotType = request.SpotType,
            VehicleType = request.VehicleType,
            Status = SpotStatus.Available,
            IsHandicapped = request.IsHandicapped,
            IsEVCharging = request.IsEVCharging,
            PricePerHour = request.PricePerHour
        };

        await _repository.AddAsync(entity);
        return Map(entity);
    }

    public async Task<SpotResponse?> ReserveAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null || entity.Status != SpotStatus.Available)
            return null;

        entity.Status = SpotStatus.Reserved;
        await _repository.UpdateAsync(entity);

        return Map(entity);
    }

    public async Task<SpotResponse?> OccupyAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null ||
            (entity.Status != SpotStatus.Available && entity.Status != SpotStatus.Reserved))
            return null;

        entity.Status = SpotStatus.Occupied;
        await _repository.UpdateAsync(entity);

        return Map(entity);
    }

    public async Task<SpotResponse?> ReleaseAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return null;

        entity.Status = SpotStatus.Available;
        await _repository.UpdateAsync(entity);

        return Map(entity);
    }

    public async Task<object> GetCountAsync(int lotId)
    {
        var spots = await _repository.GetByLotAsync(lotId);

        return new
        {
            lotId,
            totalSpots = spots.Count,
            availableSpots = spots.Count(x => x.Status == SpotStatus.Available),
            reservedSpots = spots.Count(x => x.Status == SpotStatus.Reserved),
            occupiedSpots = spots.Count(x => x.Status == SpotStatus.Occupied),
            unavailableSpots = spots.Count(x =>
                x.Status == SpotStatus.Reserved || x.Status == SpotStatus.Occupied)
        };
    }

    private static SpotResponse Map(SpotEntity x) => new()
    {
        SpotId = x.SpotId,
        LotId = x.LotId,
        SpotNumber = x.SpotNumber,
        Floor = x.Floor,
        SpotType = x.SpotType,
        VehicleType = x.VehicleType,
        Status = x.Status,
        IsHandicapped = x.IsHandicapped,
        IsEVCharging = x.IsEVCharging,
        PricePerHour = x.PricePerHour
    };
}
