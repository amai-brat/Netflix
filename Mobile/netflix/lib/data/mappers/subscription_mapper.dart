import 'package:netflix/data/models/api_subscription.dart';
import 'package:netflix/data/models/api_user_subscription.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/models/user_subscription.dart';

class SubscriptionMapper {
  SubscriptionMapper._();

  static Subscription mapSubscriptionFrom(ApiSubscription apiSubscription) {
    return Subscription(
      id: apiSubscription.id,
      name: apiSubscription.name,
      description: apiSubscription.description,
      maxResolution: apiSubscription.maxResolution,
      price: apiSubscription.price,
    );
  }

  static UserSubscription mapUserSubscriptionFrom(ApiUserSubscription apiUserSubscription) {
    return UserSubscription(
      id: apiUserSubscription.id,
      userId: apiUserSubscription.userId,
      subscriptionId: apiUserSubscription.subscriptionId,
      expiresAt: DateTime.parse(apiUserSubscription.expiresAt),
      boughtAt: DateTime.parse(apiUserSubscription.boughtAt),
      transactionId: apiUserSubscription.transactionId,
      status: switch (apiUserSubscription.status) {
        "PENDING" => UserSubscriptionStatus.pending,
        "COMPLETED" => UserSubscriptionStatus.completed,
        "FAILED" => UserSubscriptionStatus.failed,
        "CANCELLED" => UserSubscriptionStatus.cancelled,
        String() => UserSubscriptionStatus.pending,
      },
    );
  }
}
