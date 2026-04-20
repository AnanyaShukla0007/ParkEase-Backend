using Microsoft.EntityFrameworkCore;
using ParkingLot.Domain.Entities;

namespace ParkingLot.Infrastructure.Persistence;

public class ParkingLotDbContext : DbContext
{
    public ParkingLotDbContext(DbContextOptions<ParkingLotDbContext> options)
        : base(options)
    {
    }

    public DbSet<Domain.Entities.ParkingLot> ParkingLots => Set<Domain.Entities.ParkingLot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ParkingLot>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.State)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Pincode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Description)
                .HasMaxLength(500);
        });

        base.OnModelCreating(modelBuilder);
    }
}