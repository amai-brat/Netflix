import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/responses/subscriptions_response.dart';

import '../../../utils/result.dart';

class GetSubscriptionsUseCase {
  final SubscriptionRepository _subscriptionRepository;

  GetSubscriptionsUseCase({
    required SubscriptionRepository subscriptionRepository,
  }) : _subscriptionRepository = subscriptionRepository;

  Future<Result<SubscriptionsResponse>> execute() async {
    try {
      return await _subscriptionRepository.getAllSubscriptions();
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
