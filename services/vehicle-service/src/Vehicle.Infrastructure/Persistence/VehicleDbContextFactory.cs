using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Vehicle.Infrastructure.Persistence;

public class VehicleDbContextFactory : IDesignTimeDbContextFactory<VehicleDbContext>
{
    public VehicleDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VehicleDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=parkease_vehicle_db;Username=postgres;Password=AnaPassword");

        return new VehicleDbContext(optionsBuilder.Options);
    }
}
