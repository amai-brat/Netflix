import 'package:netflix/domain/models/user_subscription.dart';

class UserSubscriptionsResponse {
  final List<UserSubscription> userSubscriptions;

  UserSubscriptionsResponse({required this.userSubscriptions});
}