using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportPermanentS3Service.Data.Entities;

namespace SupportPermanentS3Service.Data.Configurations;

public class MetadataConfiguration : IEntityTypeConfiguration<Metadata>
{
    public void Configure(EntityTypeBuilder<Metadata> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(256);
    }
}