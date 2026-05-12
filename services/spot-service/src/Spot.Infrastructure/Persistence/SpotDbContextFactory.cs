using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ParkEase.Spot.Infrastructure.Persistence
{
    public class SpotDbContextFactory : IDesignTimeDbContextFactory<SpotDbContext>
    {
        public SpotDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SpotDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=parkease_spot_db;Username=postgres;Password=AnaPassword");

            return new SpotDbContext(optionsBuilder.Options);
        }
    }
}