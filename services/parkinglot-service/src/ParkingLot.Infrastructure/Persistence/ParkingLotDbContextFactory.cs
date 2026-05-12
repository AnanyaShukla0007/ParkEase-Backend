using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ParkingLot.Infrastructure.Persistence;

public class ParkingLotDbContextFactory : IDesignTimeDbContextFactory<ParkingLotDbContext>
{
    public ParkingLotDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ParkingLotDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=parkease_parkinglot_db;Username=postgres;Password=AnaPassword");

        return new ParkingLotDbContext(optionsBuilder.Options);
    }
}