using ParkEase.Spot.Application.DTOs;
using Spot.Application.Interfaces;
using ParkEase.Spot.Domain.Enums;
using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;

namespace Spot.Application.Services;

public class SpotServices : ISpotService
{
    private readonly ISpotRepository _repository;

    public SpotServices(ISpotRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SpotResponse>> GetAllAsync()
    {
        var spots = await _repository.GetAllAsync();
        return spots.Select(Map).ToList();
    }

    public async Task<SpotResponse?> GetByIdAsync(int id)
    {
        var spots = await _repository.GetAllAsync();
        var spot = spots.FirstOrDefault(x => x.SpotId == id);
        return spot == null ? null : Map(spot);
    }

    public async Task<List<SpotResponse>> GetByLotAsync(int lotId)
    {
        var spots = await _repository.GetAllAsync();

        return spots
            .Where(x => x.LotId == lotId)
            .Select(Map)
            .ToList();
    }

    public async Task<List<SpotResponse>> GetAvailableByLotAsync(int lotId)
    {
        var spots = await _repository.GetAllAsync();

        return spots
            .Where(x => x.LotId == lotId && x.Status == SpotStatus.Available)
            .Select(Map)
            .ToList();
    }

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

    private static SpotResponse Map(SpotEntity x)
    {
        return new SpotResponse
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
}