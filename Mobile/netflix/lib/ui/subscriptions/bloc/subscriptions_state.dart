import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/models/user_subscription.dart';

enum SubscriptionsStatus { initial, loading, completed }

class SubscriptionsState {
  final SubscriptionsStatus status;
  final List<Subscription> subscriptions;
  final List<UserSubscription> userSubscriptions;

  SubscriptionsState({
    required this.status,
    required this.subscriptions,
    required this.userSubscriptions,
  });

  SubscriptionsState.initial()
    : status = SubscriptionsStatus.initial,
      subscriptions = [],
      userSubscriptions = [];

  SubscriptionsState copyWith({
    SubscriptionsStatus? status,
    List<Subscription>? subscriptions,
    List<UserSubscription>? userSubscriptions,
  }) {
    return SubscriptionsState(
      status: status ?? this.status,
      subscriptions: subscriptions ?? this.subscriptions,
      userSubscriptions: userSubscriptions ?? this.userSubscriptions,
    );
  }
}
