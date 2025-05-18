import 'package:netflix/data/mappers/subscription_mapper.dart';
import 'package:netflix/data/models/api_subscription.dart';
import 'package:netflix/data/models/api_user_subscription.dart';
import 'package:netflix/data/services/subscription_service.dart';
import 'package:netflix/domain/dtos/bank_card_dto.dart';
import 'package:netflix/domain/models/user_subscription.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/responses/subscriptions_response.dart';
import 'package:netflix/domain/responses/user_subscriptions_response.dart';
import 'package:netflix/utils/result.dart';

class SubscriptionRepositoryImpl extends SubscriptionRepository {
  final SubscriptionService _subscriptionService;

  SubscriptionRepositoryImpl({required SubscriptionService subscriptionService})
    : _subscriptionService = subscriptionService;

  @override
  Future<Result<SubscriptionsResponse>> getAllSubscriptions() async {
    final apiRes = await _subscriptionService.getAllSubscriptions();

    switch (apiRes) {
      case Ok<List<ApiSubscription>>():
        return Result.ok(
          SubscriptionsResponse(
            subscriptions:
                apiRes.value
                    .map((as) => SubscriptionMapper.mapSubscriptionFrom(as))
                    .toList(),
          ),
        );
      case Error<List<ApiSubscription>>():
        return Result.error(apiRes.error);
    }
  }

  @override
  Future<Result<UserSubscriptionsResponse>> getUserSubscriptions({
    required int userId,
  }) async {
    final apiRes = await _subscriptionService.getUserSubscriptions();

    switch (apiRes) {
      case Ok<List<ApiUserSubscription>>():
        return Result.ok(
          UserSubscriptionsResponse(
            userSubscriptions:
                apiRes.value
                    .map((as) => SubscriptionMapper.mapUserSubscriptionFrom(as))
                    .toList(),
          ),
        );
      case Error<List<ApiUserSubscription>>():
        return Result.error(apiRes.error);
    }
  }

  @override
  Future<Result<void>> purchaseSubscription({
    required int userId,
    required int subscriptionId,
    required BankCardDto card,
  }) async {
    final apiRes = await _subscriptionService.buySubscription(
      subscriptionId: subscriptionId,
      card: card,
    );

    switch (apiRes) {
      case Ok<ApiUserSubscription>():
        final sub = SubscriptionMapper.mapUserSubscriptionFrom(apiRes.value);
        if (sub.status == UserSubscriptionStatus.failed) {
          return Result.error(
            "Покупка подписки не удалась. Попробуйте ещё раз",
          );
        }
        return Result.ok(null);
      case Error<ApiUserSubscription>():
        return Result.error(apiRes.error);
    }
  }

  @override
  Future<Result<void>> cancelSubscription({
    required int userId,
    required int subscriptionId,
  }) async {
    final apiRes = await _subscriptionService.cancelSubscription(
      subscriptionId: subscriptionId,
    );

    switch (apiRes) {
      case Ok<bool>():
        return Result.ok(null);
      case Error<bool>():
        return Result.error(apiRes.error);
    }
  }
}
