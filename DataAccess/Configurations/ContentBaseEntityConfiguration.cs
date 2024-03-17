using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class ContentBaseEntityConfiguration : IEntityTypeConfiguration<ContentBase>
	{
		public void Configure(EntityTypeBuilder<ContentBase> builder)
		{
			builder.HasKey(c => c.Id);

			builder.OwnsOne(c => c.AgeRatings).WithOwner();
			builder.OwnsOne(c => c.Ratings).WithOwner();
			builder.OwnsOne(c => c.TrailerInfo).WithOwner();
			builder.OwnsOne(c => c.Budget).WithOwner();

			builder.Property(c => c.Name).IsRequired();
			builder.Property(c => c.Description).IsRequired();
			builder.Property(c => c.PosterUrl).IsRequired();

			builder.HasOne(c => c.ContentType)
				.WithMany(content => content.ContentsWithType);

			builder.HasMany(c => c.Genres)
				.WithMany(g => g.Contents)
				.UsingEntity<Genre>(
					gt => gt
					.HasKey(g => g.Name)
				);

			builder.HasMany(c => c.Reviews)
				.WithOne(r => r.Content)
				.HasForeignKey(r => r.ContentId);

			builder.HasMany(c => c.AllowedSubscriptions)
				.WithMany(s => s.AccessibleContent);
		}
	}
}
