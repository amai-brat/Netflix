using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
	{
		public void Configure(EntityTypeBuilder<UserSubscription> builder)
		{
			builder.HasKey(us => us.Id);

			builder.HasOne(us => us.Subscription)
				.WithMany()
				.HasForeignKey(us => us.SubscriptionId);

			builder.HasOne(us => us.User)
				.WithMany(u => u.UserSubscriptions)
				.HasForeignKey(us => us.UserId);
		}
	}
}
