using Vehicle.Domain.Entities;
using Vehicle.Domain.Enums;
using VehicleEntity = Vehicle.Domain.Entities.Vehicle;

namespace Vehicle.Infrastructure.Persistence.Seed;

public static class VehicleSeeder
{
    public static async Task SeedAsync(VehicleDbContext context)
    {
        if (context.Vehicles.Any())
            return;

        var items = new List<VehicleEntity>
        {
            new()
            {
                OwnerId = 1,
                LicensePlate = "UP32AB1234",
                Make = "Honda",
                Model = "City",
                Color = "White",
                VehicleType = VehicleType.FourWheeler,
                IsEV = false,
                IsActive = true
            },
            new()
            {
                OwnerId = 2,
                LicensePlate = "DL01CD5678",
                Make = "Tata",
                Model = "Nexon EV",
                Color = "Blue",
                VehicleType = VehicleType.FourWheeler,
                IsEV = true,
                IsActive = true
            }
        };

        await context.Vehicles.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }
}
