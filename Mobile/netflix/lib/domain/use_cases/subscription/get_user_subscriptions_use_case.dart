import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/responses/user_subscriptions_response.dart';

import '../../../utils/result.dart';

class GetUserSubscriptionsUseCase {
  final SubscriptionRepository _subscriptionRepository;
  final AuthRepository _authRepository;

  GetUserSubscriptionsUseCase({
    required SubscriptionRepository subscriptionRepository,
    required AuthRepository authRepository
  }) : _subscriptionRepository = subscriptionRepository,
      _authRepository = authRepository;

  Future<Result<UserSubscriptionsResponse>> execute() async {
    final userId = await _authRepository.currentUserId;
    if (userId == null) {
      return Result.ok(UserSubscriptionsResponse(userSubscriptions: []));  
    }
    
    try {
      return await _subscriptionRepository.getUserSubscriptions(userId: userId);
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
