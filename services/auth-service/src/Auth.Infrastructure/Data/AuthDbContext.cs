using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext
    : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

        builder.Entity<User>(entity =>
        {
            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.VehiclePlate)
                .HasMaxLength(20);

            entity.Property(x => x.ProfilePicUrl)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Email).IsUnique();
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Token)
                .IsRequired()
                .HasMaxLength(512);

            entity.HasIndex(x => x.Token)
                .IsUnique();

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = "DRIVER",
                NormalizedName = "DRIVER"
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "MANAGER",
                NormalizedName = "MANAGER"
            },
            new IdentityRole<int>
            {
                Id = 3,
                Name = "ADMIN",
                NormalizedName = "ADMIN"
            });
    }
}