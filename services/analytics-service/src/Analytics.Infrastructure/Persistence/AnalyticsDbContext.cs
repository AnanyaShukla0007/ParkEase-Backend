using Microsoft.EntityFrameworkCore;
using Analytics.Domain.Entities;

namespace Analytics.Infrastructure.Persistence;

public class AnalyticsDbContext : DbContext
{
    public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options) : base(options)
    {
    }

    public DbSet<OccupancyLog> OccupancyLogs => Set<OccupancyLog>();
    public DbSet<DemandSignal> DemandSignals => Set<DemandSignal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OccupancyLog>(entity =>
        {
            entity.ToTable("OccupancyLogs");
            entity.HasKey(x => x.LogId);
            entity.Property(x => x.LogId).ValueGeneratedOnAdd();
            entity.Property(x => x.VehicleType).IsRequired().HasMaxLength(50);
            entity.Property(x => x.OccupancyRate).HasColumnType("numeric(5,2)");
            entity.HasIndex(x => x.LotId);
            entity.HasIndex(x => x.Timestamp);
        });

        modelBuilder.Entity<DemandSignal>(entity =>
        {
            entity.ToTable("DemandSignals");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).ValueGeneratedOnAdd();
            entity.Property(x => x.City).IsRequired().HasMaxLength(100);
            entity.Property(x => x.EventType).IsRequired().HasMaxLength(80);
            entity.Property(x => x.SearchTerm).HasMaxLength(200);
            entity.Property(x => x.Reason).HasMaxLength(250);
            entity.HasIndex(x => x.City);
            entity.HasIndex(x => x.OccurredAt);
        });
    }
}
