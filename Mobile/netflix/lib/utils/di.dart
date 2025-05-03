import 'package:get_it/get_it.dart';
import 'package:netflix/data/repositories/auth_repository_mock.dart';
import 'package:netflix/data/repositories/subscription_repository_mock.dart';
import 'package:netflix/data/services/auth_service_mock.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/use_cases/get_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/get_user_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/purchase_subscription_use_case.dart';
import 'package:netflix/domain/use_cases/signin_use_case.dart';
import 'package:netflix/domain/use_cases/signout_use_case.dart';
import 'package:netflix/domain/use_cases/signup_use_case.dart';

final GetIt locator = GetIt.instance;

void setupLocator() {
  // services
  locator.registerLazySingleton<AuthServiceMock>(() => AuthServiceMock());

  // repos
  locator.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryMock(authService: locator<AuthServiceMock>()),
  );
  locator.registerLazySingleton<SubscriptionRepository>(
    () => SubscriptionRepositoryMock(),
  );

  // use cases
  locator.registerLazySingleton(
    () => SignUpUseCase(authRepository: locator<AuthRepository>()),
  );
  locator.registerLazySingleton(
    () => SignInUseCase(authRepository: locator<AuthRepository>()),
  );
  locator.registerLazySingleton(
    () => SignOutUseCase(authRepository: locator<AuthRepository>()),
  );

  locator.registerLazySingleton(
    () => GetSubscriptionsUseCase(
      subscriptionRepository: locator<SubscriptionRepository>(),
    ),
  );
  locator.registerLazySingleton(
    () => GetUserSubscriptionsUseCase(
      subscriptionRepository: locator<SubscriptionRepository>(),
      authRepository: locator<AuthRepository>(),
    ),
  );
  locator.registerLazySingleton(
    () => PurchaseSubscriptionUseCase(
      subscriptionRepository: locator<SubscriptionRepository>(),
      authRepository: locator<AuthRepository>(),
    ),
  );
}
