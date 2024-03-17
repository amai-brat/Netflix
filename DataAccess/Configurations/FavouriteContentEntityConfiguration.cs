using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class FavouriteContentEntityConfiguration : IEntityTypeConfiguration<FavouriteContent>
	{
		public void Configure(EntityTypeBuilder<FavouriteContent> builder)
		{
			builder.HasKey(f => new { f.UserId, f.ContentId });

			builder.HasOne(f => f.User)
				.WithMany(u => u.FavouriteContents)
				.HasForeignKey(x => x.UserId);

			builder.HasOne(f => f.Content)
				.WithMany()
				.HasForeignKey(f => f.ContentId);
		}
	}
}
