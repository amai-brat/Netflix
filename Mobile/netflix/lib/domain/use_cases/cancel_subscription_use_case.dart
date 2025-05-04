import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';

import '../../utils/result.dart';

class CancelSubscriptionUseCase {
  final SubscriptionRepository _subscriptionRepository;
  final AuthRepository _authRepository;

  CancelSubscriptionUseCase({
    required SubscriptionRepository subscriptionRepository,
    required AuthRepository authRepository,
  }) : _subscriptionRepository = subscriptionRepository,
       _authRepository = authRepository;

  Future<Result<void>> execute({required int subscriptionId}) async {
    final userId = await _authRepository.currentUserId;
    if (userId == null) {
      return Result.error('Пользователь не авторизован');
    }

    try {
      return await _subscriptionRepository.cancelSubscription(
        userId: userId,
        subscriptionId: subscriptionId,
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
