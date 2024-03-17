using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.HasKey(r => r.Id);

			builder.Property(r => r.Text).IsRequired();
			builder.Property(r => r.WrittenAt).IsRequired();

			builder.HasMany(r => r.Comments)
				.WithOne(r => r.Review)
				.HasForeignKey(r => r.ReviewId);
		}
	}
}
