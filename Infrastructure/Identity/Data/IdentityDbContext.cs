using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Data;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<AppUser, AppRole, long>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
        SeedRoles(builder);
        
        base.OnModelCreating(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<AppRole>().HasData(
            new AppRole("user"){Id = 1}, 
            new AppRole("moderator"){Id = 2}, 
            new AppRole("admin"){Id = 3},
            new AppRole("support") { Id = 4}
            );
    }
}