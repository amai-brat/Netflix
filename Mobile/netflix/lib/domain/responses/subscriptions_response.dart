import 'package:netflix/domain/models/subscription.dart';

class SubscriptionsResponse {
  final List<Subscription> subscriptions;

  SubscriptionsResponse({required this.subscriptions});
}