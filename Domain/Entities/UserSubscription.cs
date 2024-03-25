using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class UserSubscription
	{
		public User User { get; set; } = null!;
		public long UserId { get; set; }

		public Subscription Subscription { get; set; } = null!;
		public int SubscriptionId { get; set; }

		public DateTimeOffset ExpiresAt { get; set; }
		public DateTimeOffset BoughtAt { get; set; }
	}
}
