using Microsoft.EntityFrameworkCore;
using VehicleEntity = Vehicle.Domain.Entities.Vehicle;

namespace Vehicle.Infrastructure.Persistence;

public class VehicleDbContext : DbContext
{
    public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
        : base(options)
    {
    }

    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleEntity>(entity =>
        {
            entity.ToTable("Vehicles");

            entity.HasKey(x => x.VehicleId);

            entity.Property(x => x.VehicleId)
                .ValueGeneratedOnAdd();

            entity.Property(x => x.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.Color)
                .IsRequired()
                .HasMaxLength(30);

            entity.HasIndex(x => x.OwnerId);
            entity.HasIndex(x => new { x.OwnerId, x.LicensePlate }).IsUnique();
        });
    }
}
