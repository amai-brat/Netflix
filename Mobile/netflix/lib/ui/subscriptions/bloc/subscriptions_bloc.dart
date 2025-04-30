import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/use_cases/get_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/get_user_subscriptions_use_case.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_event.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_state.dart';

class SubscriptionsBloc extends Bloc<SubscriptionsEvent, SubscriptionsState> {
  final GetSubscriptionsUseCase _getSubscriptionsUseCase;
  final GetUserSubscriptionsUseCase _getUserSubscriptionsUseCase;

  SubscriptionsBloc({
    required GetSubscriptionsUseCase getSubscriptionsUseCase,
    required GetUserSubscriptionsUseCase getUserSubscriptionsUseCase,
  }) : _getSubscriptionsUseCase = getSubscriptionsUseCase,
       _getUserSubscriptionsUseCase = getUserSubscriptionsUseCase,
       super(SubscriptionsState.initial());


}
