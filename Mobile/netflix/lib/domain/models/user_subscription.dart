class UserSubscription {
  final int id;
  final int userId;
  final int subscriptionId;
  final DateTime expiresAt;
  final DateTime boughtAt;
  final String? transactionId;
  final UserSubscriptionStatus status;

  UserSubscription({
    required this.id,
    required this.userId,
    required this.subscriptionId,
    required this.expiresAt,
    required this.boughtAt,
    required this.transactionId,
    required this.status,
  });
}

enum UserSubscriptionStatus { pending, completed, failed, cancelled }
