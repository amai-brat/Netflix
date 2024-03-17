using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class UsersReviewsEntityConfiguration : IEntityTypeConfiguration<UsersReviews>
	{
		public void Configure(EntityTypeBuilder<UsersReviews> builder)
		{
			builder.HasKey(ur => new { ur.UserId, ur.ReviewId });

			builder.HasOne(ur => ur.User)
				.WithMany(u => u.ScoredReviews)
				.HasForeignKey(ur => ur.UserId);

			builder.HasOne(ur => ur.Review)
				.WithMany(r => r.RatedByUsers)
				.HasForeignKey(ur => ur.ReviewId);
		}
	}
}
