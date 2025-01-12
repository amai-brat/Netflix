using Microsoft.EntityFrameworkCore;
using SupportPermanentS3Service.Data.Configurations;
using SupportPermanentS3Service.Data.Entities;
using File = SupportPermanentS3Service.Data.Entities.File;

namespace SupportPermanentS3Service.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<File> Files => Set<File>();
    public DbSet<Metadata> Metadata => Set<Metadata>();
    public DbSet<MetadataValue> Values => Set<MetadataValue>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new FileConfiguration());
        modelBuilder.ApplyConfiguration(new MetadataConfiguration());
        modelBuilder.ApplyConfiguration(new MetadataValueConfiguration());
    }
}