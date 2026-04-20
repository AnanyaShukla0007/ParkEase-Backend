using ParkingLot.Application.DTOs;
using ParkingLot.Application.Interfaces;
using ParkingLot.Domain.Entities;
using ParkingLot.Application.Helpers;

namespace ParkingLot.Application.Services;

public class ParkingLotService : IParkingLotService
{
    private readonly IParkingLotRepository _repo;

    public ParkingLotService(IParkingLotRepository repo)
    {
        _repo = repo;
    }

    public async Task<ParkingLotResponse> CreateAsync(CreateParkingLotRequest request)
    {
        var entity = new Domain.Entities.ParkingLot
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Pincode = request.Pincode,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            TotalSpots = request.TotalSpots,
            AvailableSpots = request.TotalSpots,
            ManagerId = request.ManagerId,
            OpenTime = request.OpenTime,
            CloseTime = request.CloseTime,
            ImageUrl = request.ImageUrl,
            IsActive = true,
            IsApproved = false,
            IsOpen = true
        };

        var saved = await _repo.AddAsync(entity);
        return Map(saved);
    }

    public async Task<ParkingLotResponse> GetByIdAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Parking lot not found.");

        return Map(item);
    }

    public async Task<List<ParkingLotResponse>> GetAllAsync()
        => (await _repo.GetAllAsync()).Select(Map).ToList();

    public async Task<List<ParkingLotResponse>> GetByCityAsync(string city)
        => (await _repo.GetByCityAsync(city)).Select(Map).ToList();

    public async Task<List<ParkingLotResponse>> GetByManagerAsync(int managerId)
        => (await _repo.GetByManagerAsync(managerId)).Select(Map).ToList();

    public async Task<List<NearbyParkingLotResponse>> GetNearbyAsync(double lat, double lng)
    {
        var data = await _repo.GetAllAsync();

        return data
            .Where(x => x.IsActive && x.IsApproved && x.IsOpen)
            .Select(x =>
            {
                var km = GeoDistanceHelper.CalculateKm(
                    lat, lng, x.Latitude, x.Longitude);

                return new NearbyParkingLotResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Pincode = x.Pincode,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    TotalSpots = x.TotalSpots,
                    AvailableSpots = x.AvailableSpots,
                    IsActive = x.IsActive,
                    IsApproved = x.IsApproved,
                    IsOpen = x.IsOpen,
                    ManagerId = x.ManagerId,
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime,
                    ImageUrl = x.ImageUrl,
                    CreatedAt = x.CreatedAt,
                    DistanceKm = Math.Round(km, 2),
                    EstimatedMinutes = Math.Max(1, (int)Math.Ceiling(km * 3))
                };
            })
            .OrderBy(x => x.DistanceKm)
            .ToList();
    }

    public async Task<ParkingLotResponse> UpdateAsync(int id, UpdateParkingLotRequest request)
    {
        var item = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Parking lot not found.");

        if (!string.IsNullOrWhiteSpace(request.Name)) item.Name = request.Name;
        if (!string.IsNullOrWhiteSpace(request.Description)) item.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.Address)) item.Address = request.Address;
        if (!string.IsNullOrWhiteSpace(request.City)) item.City = request.City;
        if (!string.IsNullOrWhiteSpace(request.State)) item.State = request.State;
        if (!string.IsNullOrWhiteSpace(request.Pincode)) item.Pincode = request.Pincode;
        if (request.TotalSpots.HasValue) item.TotalSpots = request.TotalSpots.Value;
        if (request.IsOpen.HasValue) item.IsOpen = request.IsOpen.Value;
        if (request.ImageUrl is not null) item.ImageUrl = request.ImageUrl;
        if (request.OpenTime.HasValue) item.OpenTime = request.OpenTime.Value;
        if (request.CloseTime.HasValue) item.CloseTime = request.CloseTime.Value;

        item.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(item);
        return Map(item);
    }

    public async Task ToggleApprovalAsync(int id, bool approved)
    {
        var item = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Parking lot not found.");

        item.IsApproved = approved;
        await _repo.UpdateAsync(item);
    }

    public async Task ToggleActiveAsync(int id, bool active)
    {
        var item = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Parking lot not found.");

        item.IsActive = active;
        await _repo.UpdateAsync(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Parking lot not found.");

        await _repo.DeleteAsync(item);
    }

    private static ParkingLotResponse Map(Domain.Entities.ParkingLot x)
    {
        return new ParkingLotResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Address = x.Address,
            City = x.City,
            State = x.State,
            Pincode = x.Pincode,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            TotalSpots = x.TotalSpots,
            AvailableSpots = x.AvailableSpots,
            IsActive = x.IsActive,
            IsApproved = x.IsApproved,
            IsOpen = x.IsOpen,
            ManagerId = x.ManagerId,
            OpenTime = x.OpenTime,
            CloseTime = x.CloseTime,
            ImageUrl = x.ImageUrl,
            CreatedAt = x.CreatedAt
        };
    }
}