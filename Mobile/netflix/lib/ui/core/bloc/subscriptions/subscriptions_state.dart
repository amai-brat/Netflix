import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/models/user_subscription.dart';

enum SubscriptionsStatus { initial, loading, completed }

class SubscriptionsState {
  final SubscriptionsStatus status;
  final List<Subscription> subscriptions;
  final List<UserSubscription> userSubscriptions;
  final String error;

  SubscriptionsState({
    required this.status,
    required this.subscriptions,
    required this.userSubscriptions,
    required this.error
  });

  SubscriptionsState.initial()
    : status = SubscriptionsStatus.initial,
      subscriptions = [],
      userSubscriptions = [],
      error = '';

  SubscriptionsState copyWith({
    SubscriptionsStatus? status,
    List<Subscription>? subscriptions,
    List<UserSubscription>? userSubscriptions,
    String? error
  }) {
    return SubscriptionsState(
      status: status ?? this.status,
      subscriptions: subscriptions ?? this.subscriptions,
      userSubscriptions: userSubscriptions ?? this.userSubscriptions,
      error: error ?? this.error
    );
  }
}
