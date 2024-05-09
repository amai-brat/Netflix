using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class UserEntityConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);
			builder.HasIndex(u => u.Email).IsUnique();
			
			builder.Property(u => u.Nickname).IsRequired();
			builder.Property(u => u.Email).IsRequired();

			builder.HasMany(u => u.Reviews)
				.WithOne(r => r.User)
				.HasForeignKey(r => r.UserId);

			builder.HasMany(u => u.Comments)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);

			builder.HasMany(u => u.ScoredComments)
				.WithMany(c => c.ScoredByUsers)
				.UsingEntity<Dictionary<string, object>>(
					"CommentUser",
					l => l.HasOne<Comment>().WithMany().HasForeignKey("ScoredCommentsId"),
					r => r.HasOne<User>().WithMany().HasForeignKey("ScoredByUsersId"),
					je =>
					{
						je.HasKey("ScoredByUsersId", "ScoredCommentsId");
					}); ;
		}
	}
}
