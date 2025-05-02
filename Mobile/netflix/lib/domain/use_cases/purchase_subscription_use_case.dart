import 'package:netflix/domain/dtos/bank_card_dto.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';

import '../../utils/result.dart';

class PurchaseSubscriptionUseCase {
  final SubscriptionRepository _subscriptionRepository;
  final AuthRepository _authRepository;

  PurchaseSubscriptionUseCase({
    required SubscriptionRepository subscriptionRepository,
    required AuthRepository authRepository,
  }) : _subscriptionRepository = subscriptionRepository,
       _authRepository = authRepository;

  Future<Result<void>> execute({
    required int subscriptionId,
    required BankCardDto card,
  }) async {
    final userId = await _authRepository.currentUserId;
    if (userId == null) {
      return Result.error('Пользователь не авторизован');
    }

    try {
      return await _subscriptionRepository.purchaseSubscription(
        userId: userId,
        card: card,
        subscriptionId: subscriptionId,
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
