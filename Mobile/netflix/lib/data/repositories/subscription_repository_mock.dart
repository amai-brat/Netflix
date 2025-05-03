import 'dart:math';

import 'package:netflix/domain/dtos/bank_card_dto.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/models/user_subscription.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/responses/subscriptions_response.dart';
import 'package:netflix/domain/responses/user_subscriptions_response.dart';
import 'package:netflix/utils/result.dart';

class SubscriptionRepositoryMock extends SubscriptionRepository {
  final _subscriptions = [
    Subscription(
      id: 1,
      name: 'Фильмы',
      description:
          'Все фильмы на сервисе Netflix будут доступны после приобретения этой подписки',
      maxResolution: 2160,
      price: 300,
    ),
    Subscription(
      id: 2,
      name: 'Сериалы',
      description: 'Все сериалы только в этой подписке',
      maxResolution: 1080,
      price: 350,
    ),
    Subscription(
      id: 3,
      name: 'Мультфильмы',
      description: 'Мультфильмы для всех возрастов только в данной подписке',
      maxResolution: 720,
      price: 228,
    ),
  ];

  final _userSubscriptions = [
    UserSubscription(
      id: 1,
      userId: 1,
      subscriptionId: 2,
      expiresAt: DateTime.parse('2025-05-14T15:31:19.115Z'),
      boughtAt: DateTime.parse('2025-04-14T15:31:19.115Z'),
      transactionId: '03217d2b-b1b9-4801-a91d-f49481af76f9',
      status: UserSubscriptionStatus.completed,
    ),
  ];

  @override
  Future<Result<SubscriptionsResponse>> getAllSubscriptions() async {
    return Result.ok(SubscriptionsResponse(subscriptions: _subscriptions));
  }

  @override
  Future<Result<UserSubscriptionsResponse>> getUserSubscriptions({
    required int userId,
  }) async {
    final userSubs =
        _userSubscriptions.where((us) => us.userId == userId).toList();

    return Result.ok(UserSubscriptionsResponse(userSubscriptions: userSubs));
  }

  @override
  Future<Result<void>> purchaseSubscription({
    required int userId,
    required int subscriptionId,
    required BankCardDto card
  }) async {
    final now = DateTime.now();
    _userSubscriptions.add(
      UserSubscription(
        id: Random().nextInt(10_000),
        userId: userId,
        subscriptionId: subscriptionId,
        expiresAt: now.add(Duration(days: 30)),
        boughtAt: now,
        transactionId: null,
        status: UserSubscriptionStatus.completed,
      ),
    );

    return Result.ok(null);
  }
}
