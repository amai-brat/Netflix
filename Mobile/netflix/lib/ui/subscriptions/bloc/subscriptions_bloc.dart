import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/dtos/bank_card_dto.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/responses/subscriptions_response.dart';
import 'package:netflix/domain/responses/user_subscriptions_response.dart';
import 'package:netflix/domain/use_cases/get_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/get_user_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/purchase_subscription_use_case.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_event.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_state.dart';
import 'package:netflix/utils/result.dart';

import '../../../utils/di.dart';

class SubscriptionsBloc extends Bloc<SubscriptionsEvent, SubscriptionsState> {
  final GetSubscriptionsUseCase _getSubscriptionsUseCase;
  final GetUserSubscriptionsUseCase _getUserSubscriptionsUseCase;
  final PurchaseSubscriptionUseCase _purchaseSubscriptionUseCase;

  SubscriptionsBloc({
    required GetSubscriptionsUseCase getSubscriptionsUseCase,
    required GetUserSubscriptionsUseCase getUserSubscriptionsUseCase,
    required PurchaseSubscriptionUseCase purchaseSubscriptionUseCase,
  }) : _getSubscriptionsUseCase = getSubscriptionsUseCase,
       _getUserSubscriptionsUseCase = getUserSubscriptionsUseCase,
       _purchaseSubscriptionUseCase = purchaseSubscriptionUseCase,
       super(SubscriptionsState.initial()) {
    on<SubscriptionsPageOpened>(_onSubscriptionsPageOpened);
    on<PurchasePressed>(_onPurchasePressed);
    on<UserSubscriptionsUpdated>(_onUserSubscriptionsUpdated);
  }

  static SubscriptionsBloc createViaLocator() {
    return SubscriptionsBloc(
      getSubscriptionsUseCase: locator<GetSubscriptionsUseCase>(),
      getUserSubscriptionsUseCase: locator<GetUserSubscriptionsUseCase>(),
      purchaseSubscriptionUseCase: locator<PurchaseSubscriptionUseCase>(),
    );
  }

  bool isPurchased(Subscription sub) {
    return state.userSubscriptions.any((us) => us.subscriptionId == sub.id);
  }

  FutureOr<void> _onSubscriptionsPageOpened(
    SubscriptionsPageOpened event,
    Emitter<SubscriptionsState> emit,
  ) async {
    final subsResult = await _getSubscriptionsUseCase.execute();
    switch (subsResult) {
      case Ok<SubscriptionsResponse>():
        emit(
          state.copyWith(
            subscriptions: subsResult.value.subscriptions,
            error: '',
          ),
        );
      case Error<SubscriptionsResponse>():
        emit(state.copyWith(error: subsResult.error));
    }

    final userSubsResult = await _getUserSubscriptionsUseCase.execute();
    switch (userSubsResult) {
      case Ok<UserSubscriptionsResponse>():
        emit(
          state.copyWith(
            userSubscriptions: userSubsResult.value.userSubscriptions,
            error: '',
          ),
        );
      case Error<UserSubscriptionsResponse>():
        emit(state.copyWith(error: userSubsResult.error));
    }
  }

  FutureOr<void> _onPurchasePressed(
    PurchasePressed event,
    Emitter<SubscriptionsState> emit,
  ) async {
    final result = await _purchaseSubscriptionUseCase.execute(
      subscriptionId: event.subscriptionId,
      card: BankCardDto(
        cardNumber: event.cardNumber,
        cardOwner: event.cardOwner,
        validThru: event.validThru,
        cvc: event.cvc,
      ),
    );

    switch (result) {
      case Ok<void>():
        add(UserSubscriptionsUpdated());
        emit(state.copyWith(error: ''));
      case Error<void>():
        emit(state.copyWith(error: result.error));
    }
  }

  FutureOr<void> _onUserSubscriptionsUpdated(
    UserSubscriptionsUpdated event,
    Emitter<SubscriptionsState> emit,
  ) async {
    final userSubsResult = await _getUserSubscriptionsUseCase.execute();
    switch (userSubsResult) {
      case Ok<UserSubscriptionsResponse>():
        emit(
          state.copyWith(
            userSubscriptions: userSubsResult.value.userSubscriptions,
            error: '',
          ),
        );
      case Error<UserSubscriptionsResponse>():
        emit(state.copyWith(error: userSubsResult.error));
    }
  }
}
