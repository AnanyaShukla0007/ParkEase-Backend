using ParkingLot.Domain.Entities;

namespace ParkingLot.Application.Interfaces;

public interface IParkingLotRepository
{
    Task<Domain.Entities.ParkingLot> AddAsync(Domain.Entities.ParkingLot lot);

    Task<Domain.Entities.ParkingLot?> GetByIdAsync(int id);

    Task<List<Domain.Entities.ParkingLot>> GetAllAsync();

    Task<List<Domain.Entities.ParkingLot>> GetByCityAsync(string city);

    Task<List<Domain.Entities.ParkingLot>> GetByManagerAsync(int managerId);

    Task UpdateAsync(Domain.Entities.ParkingLot lot);

    Task DeleteAsync(Domain.Entities.ParkingLot lot);
}