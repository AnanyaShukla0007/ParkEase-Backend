using ParkingLot.Application.DTOs;

namespace ParkingLot.Application.Interfaces;

public interface IParkingLotService
{
    Task<ParkingLotResponse> CreateAsync(CreateParkingLotRequest request);

    Task<ParkingLotResponse> GetByIdAsync(int id);

    Task<List<ParkingLotResponse>> GetAllAsync();

    Task<List<ParkingLotResponse>> GetByCityAsync(string city);

    Task<List<ParkingLotResponse>> GetByManagerAsync(int managerId);

    Task<List<NearbyParkingLotResponse>> GetNearbyAsync(double lat, double lng);

    Task<ParkingLotResponse> UpdateAsync(int id, UpdateParkingLotRequest request);

    Task ToggleApprovalAsync(int id, bool approved);

    Task ToggleActiveAsync(int id, bool active);

    Task DeleteAsync(int id);
}