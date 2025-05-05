import 'package:get_it/get_it.dart';
import 'package:netflix/data/repositories/auth_repository_mock.dart';
import 'package:netflix/data/repositories/favorite_repository_mock.dart';
import 'package:netflix/data/repositories/personal_info_repository.dart';
import 'package:netflix/data/repositories/subscription_repository_mock.dart';
import 'package:netflix/data/repositories/content_repository_mock.dart';
import 'package:netflix/data/repositories/content_type_repository_mock.dart';
import 'package:netflix/data/repositories/genre_repository_mock.dart';
import 'package:netflix/data/services/auth_service_mock.dart';
import 'package:netflix/data/services/personal_info_service_mock.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/favorite_repository.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/use_cases/change_birthdate_use_case.dart';
import 'package:netflix/domain/use_cases/change_email_use_case.dart';
import 'package:netflix/domain/use_cases/change_password_use_case.dart';
import 'package:netflix/domain/use_cases/get_favorite_by_filter_use_case.dart';
import 'package:netflix/domain/use_cases/get_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/get_user_info_use_case.dart';
import 'package:netflix/domain/use_cases/get_user_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/purchase_subscription_use_case.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/repositories/content_type_repository.dart';
import 'package:netflix/domain/repositories/genre_repository.dart';
import 'package:netflix/domain/use_cases/get_all_content_types_use_case.dart';
import 'package:netflix/domain/use_cases/get_all_genres_use_case.dart';
import 'package:netflix/domain/use_cases/get_content_by_filter_use_case.dart';
import 'package:netflix/domain/use_cases/signin_use_case.dart';
import 'package:netflix/domain/use_cases/signout_use_case.dart';
import 'package:netflix/domain/use_cases/signup_use_case.dart';

final GetIt locator = GetIt.instance;

void setupLocator() {
  // services
  locator.registerLazySingleton<AuthServiceMock>(() => AuthServiceMock());
  locator.registerLazySingleton<PersonalInfoServiceMock>(() => PersonalInfoServiceMock());

  // repos
  locator.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryMock(authService: locator<AuthServiceMock>()),
  );
  locator.registerLazySingleton<SubscriptionRepository>(
    () => SubscriptionRepositoryMock(),
  );

  locator.registerLazySingleton<ContentRepository>(
        () => ContentRepositoryMock(),
  );
  locator.registerLazySingleton<GenreRepository>(
        () => GenreRepositoryMock(),
  );
  locator.registerLazySingleton<ContentTypeRepository>(
        () => ContentTypeRepositoryMock(),
  );
  locator.registerLazySingleton<FavoriteRepository>(
          () => FavoriteRepositoryMock(),
  );
  locator.registerLazySingleton<PersonalInfoRepository>(
        () => PersonalInfoRepositoryImpl(service: locator<PersonalInfoServiceMock>()),
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
        () => GetContentByFilterUseCase(contentRepository: locator<ContentRepository>()),
  );
  locator.registerLazySingleton(
        () => GetAllGenresUseCase(genreRepository: locator<GenreRepository>()),
  );
  locator.registerLazySingleton(
        () => GetAllContentTypesUseCase(contentTypeRepository: locator<ContentTypeRepository>()),
  );
  locator.registerLazySingleton(
        () => GetFavoriteByFilterUseCase(favoriteRepository: locator<FavoriteRepository>()),
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

  locator.registerLazySingleton(
    () => ChangeBirthDateUseCase(repository: locator<PersonalInfoRepository>())
  );
  locator.registerLazySingleton(
    () => ChangeEmailUseCase(repository: locator<PersonalInfoRepository>())
  );
  locator.registerLazySingleton(
    () => ChangePasswordUseCase(repository: locator<PersonalInfoRepository>())
  );
  locator.registerLazySingleton(
    () => GetUserInfoUseCase(repository: locator<PersonalInfoRepository>())
  );
}
