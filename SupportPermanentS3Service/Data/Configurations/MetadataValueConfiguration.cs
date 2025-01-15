using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportPermanentS3Service.Data.Entities;

namespace SupportPermanentS3Service.Data.Configurations;

public class MetadataValueConfiguration : IEntityTypeConfiguration<MetadataValue>
{
    public void Configure(EntityTypeBuilder<MetadataValue> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value)
            .HasMaxLength(256);
        
        builder.HasOne(x => x.File)
            .WithMany()
            .HasForeignKey(x => x.FileId);
        
        builder.HasOne(x => x.Metadata)
            .WithMany()
            .HasForeignKey(x => x.MetadataId);
    }
}