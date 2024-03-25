using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class EpisodeEntityConfiguration : IEntityTypeConfiguration<Episode>
	{
		public void Configure(EntityTypeBuilder<Episode> builder)
		{
			builder.HasKey(e => e.Id);

			builder.Property(e => e.VideoUrl).IsRequired();

			builder.HasOne(e => e.SeasonInfo)
				.WithMany(si => si.Episodes);
		}
	}
}
