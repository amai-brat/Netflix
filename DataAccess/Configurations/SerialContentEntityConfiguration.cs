using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class SerialContentEntityConfiguration : IEntityTypeConfiguration<SerialContent>
	{
		public void Configure(EntityTypeBuilder<SerialContent> builder)
		{
			builder.ToTable("serial_contents");

			builder.OwnsOne(s => s.YearRange).WithOwner();

			builder.HasMany(s => s.SeasonInfos)
				.WithOne(si => si.SerialContent);
		}
	}
}
