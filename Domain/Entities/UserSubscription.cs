namespace Domain.Entities
{
	public class UserSubscription
	{
		public int Id { get; set; }
		public User User { get; set; } = null!;
		public long UserId { get; set; }

		public Subscription Subscription { get; set; } = null!;
		public int SubscriptionId { get; set; }

		public DateTimeOffset ExpiresAt { get; set; }
		public DateTimeOffset BoughtAt { get; set; }

		public Guid? TransactionId { get; set; }

		public UserSubscriptionStatus Status { get; set; }
	}

	public enum UserSubscriptionStatus
	{
		Pending = 0,
		Completed = 1,
		Failed = 2
	}
}
