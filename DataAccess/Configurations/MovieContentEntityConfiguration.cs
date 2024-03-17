using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class MovieContentEntityConfiguration : IEntityTypeConfiguration<MovieContent>
	{
		public void Configure(EntityTypeBuilder<MovieContent> builder)
		{
			builder.ToTable("movie_contents");

			builder.Property(m => m.MovieLength).IsRequired();
			builder.Property(m => m.ReleaseDate).IsRequired();
		}
	}
}
