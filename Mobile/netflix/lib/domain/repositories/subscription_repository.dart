import 'package:netflix/domain/dtos/bank_card_dto.dart';
import 'package:netflix/domain/responses/subscriptions_response.dart';
import 'package:netflix/domain/responses/user_subscriptions_response.dart';

import '../../utils/result.dart';

abstract class SubscriptionRepository {
  Future<Result<SubscriptionsResponse>> getAllSubscriptions();

  Future<Result<UserSubscriptionsResponse>> getUserSubscriptions({
    required int userId,
  });

  Future<Result<void>> purchaseSubscription({
    required int userId,
    required int subscriptionId,
    required BankCardDto card,
  });

  Future<Result<void>> cancelSubscription({
    required int userId,
    required int subscriptionId,
  });
}
