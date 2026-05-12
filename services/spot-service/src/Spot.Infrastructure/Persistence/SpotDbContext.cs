using Microsoft.EntityFrameworkCore;
using SpotEntity = ParkEase.Spot.Domain.Entities.Spot;

namespace ParkEase.Spot.Infrastructure.Persistence
{
    public class SpotDbContext : DbContext
    {
        public SpotDbContext(DbContextOptions<SpotDbContext> options)
            : base(options)
        {
        }

        public DbSet<SpotEntity> Spots => Set<SpotEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpotEntity>(entity =>
            {
                entity.HasKey(x => x.SpotId);

                entity.Property(x => x.SpotNumber)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(x => x.PricePerHour)
                      .HasColumnType("numeric(18,2)");

                entity.HasIndex(x => new { x.LotId, x.SpotNumber })
                      .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}