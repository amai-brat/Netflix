using Domain.Entities;
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
				.WithMany(content => content.ContentsWithType)
				.HasForeignKey(c => c.ContentTypeId);

			builder.HasMany(c => c.Genres)
				.WithMany(g => g.Contents)
				.UsingEntity<Dictionary<string, object>>(
					"ContentBaseGenre",
					r => r.HasOne<Genre>().WithMany().HasForeignKey("GenreId"),
					l => l.HasOne<ContentBase>().WithMany().HasForeignKey("ContentBaseId"),
					je =>
					{
						je.HasKey("GenreId", "ContentBaseId");
					});

			builder.HasMany(c => c.Reviews)
				.WithOne(r => r.Content)
				.HasForeignKey(r => r.ContentId);

			builder.HasMany(c => c.AllowedSubscriptions)
				.WithMany(s => s.AccessibleContent)
				.UsingEntity<Dictionary<string, object>>(
					"ContentBaseSubscription",
					r => r.HasOne<Subscription>().WithMany().HasForeignKey("SubscriptionId"),
					l => l.HasOne<ContentBase>().WithMany().HasForeignKey("AccessibleContentId"),
					je =>
					{
						je.HasKey("SubscriptionId", "AccessibleContentId");
					});
		}
	}
}
