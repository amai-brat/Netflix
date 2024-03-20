using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class UserEntityConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);

			builder.Property(u => u.Nickname).IsRequired();
			builder.Property(u => u.Email).IsRequired();
			builder.Property(u => u.Password).IsRequired();
			builder.Property(u => u.Role).IsRequired().HasDefaultValue("user");

			builder.HasMany(u => u.Reviews)
				.WithOne(r => r.User)
				.HasForeignKey(r => r.UserId);

			builder.HasMany(u => u.Comments)
				.WithOne(c => c.User)
				.HasForeignKey(c => c.UserId);

			builder.HasMany(u => u.ScoredComments)
				.WithMany(c => c.ScoredByUsers);
		}
	}
}
