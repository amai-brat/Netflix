import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get_it/get_it.dart';
import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/repositories/auth_repository_impl.dart';
import 'package:netflix/data/repositories/favorite_repository_mock.dart';
import 'package:netflix/data/repositories/personal_info_repository.dart';
import 'package:netflix/data/repositories/reviews_repository.dart';
import 'package:netflix/data/repositories/subscription_repository_mock.dart';
import 'package:netflix/data/repositories/content_repository_mock.dart';
import 'package:netflix/data/repositories/content_type_repository_mock.dart';
import 'package:netflix/data/repositories/genre_repository_mock.dart';
import 'package:netflix/data/services/auth_service_mock.dart';
import 'package:netflix/data/services/personal_info_service_mock.dart';
import 'package:netflix/data/services/reviews_service_mock.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/repositories/favorite_repository.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/domain/repositories/reviews_repository.dart';
import 'package:netflix/domain/repositories/subscription_repository.dart';
import 'package:netflix/domain/use_cases/user/change_birthdate_use_case.dart';
import 'package:netflix/domain/use_cases/user/change_email_use_case.dart';
import 'package:netflix/domain/use_cases/user/change_password_use_case.dart';
import 'package:netflix/domain/use_cases/user/get_reviews_use_case.dart';
import 'package:netflix/domain/use_cases/user/get_total_reviews_pages_use_case.dart';
import 'package:netflix/domain/use_cases/user/get_user_info_use_case.dart';
import 'package:netflix/domain/use_cases/subscription/cancel_subscription_use_case.dart';
import 'package:netflix/domain/use_cases/content/get_sections_use_case.dart';
import 'package:netflix/domain/use_cases/content/get_content_by_id_use_case.dart';
import 'package:netflix/domain/use_cases/content/get_favorite_by_filter_use_case.dart';
import 'package:netflix/domain/use_cases/subscription/get_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/subscription/get_user_subscriptions_use_case.dart';
import 'package:netflix/domain/use_cases/subscription/purchase_subscription_use_case.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/repositories/content_type_repository.dart';
import 'package:netflix/domain/repositories/genre_repository.dart';
import 'package:netflix/domain/use_cases/content/get_all_content_types_use_case.dart';
import 'package:netflix/domain/use_cases/content/get_all_genres_use_case.dart';
import 'package:netflix/domain/use_cases/content/get_content_by_filter_use_case.dart';
import 'package:netflix/domain/use_cases/auth/signin_use_case.dart';
import 'package:netflix/domain/use_cases/auth/signout_use_case.dart';
import 'package:netflix/domain/use_cases/auth/signup_use_case.dart';
import 'package:netflix/utils/consts.dart';

import '../data/services/auth_service.dart';

final GetIt locator = GetIt.instance;

void setupLocator() {
  // etc
  locator.registerLazySingleton<FlutterSecureStorage>(
    () => FlutterSecureStorage(
      aOptions: const AndroidOptions(encryptedSharedPreferences: true),
    ),
  );

  // services
  locator.registerLazySingleton<GraphQLClient>(() {
    final HttpLink httpLink = HttpLink(dotenv.env['GRAPHQL_SERVER']!);
    final AuthLink authLink = AuthLink(
      getToken:
          () async =>
              'Bearer ${await locator<FlutterSecureStorage>().read(key: Consts.accessToken)}',
    );

    final Link link = authLink.concat(httpLink);
    return GraphQLClient(cache: GraphQLCache(), link: link);
  });
  locator.registerLazySingleton<AuthServiceMock>(() => AuthServiceMock());
  locator.registerLazySingleton<AuthService>(
    () => AuthService(locator<GraphQLClient>()),
  );
  locator.registerLazySingleton<PersonalInfoServiceMock>(
    () => PersonalInfoServiceMock(),
  );
  locator.registerLazySingleton<ReviewsServiceMock>(() => ReviewsServiceMock());

  // repos
  locator.registerLazySingleton<AuthRepository>(
    () => AuthRepositoryImpl(
      authService: locator<AuthService>(),
      secureStorage: locator<FlutterSecureStorage>(),
    ),
  );
  locator.registerLazySingleton<SubscriptionRepository>(
    () => SubscriptionRepositoryMock(),
  );

  locator.registerLazySingleton<ContentRepository>(
    () => ContentRepositoryMock(),
  );
  locator.registerLazySingleton<GenreRepository>(() => GenreRepositoryMock());
  locator.registerLazySingleton<ContentTypeRepository>(
    () => ContentTypeRepositoryMock(),
  );
  locator.registerLazySingleton<FavoriteRepository>(
    () => FavoriteRepositoryMock(),
  );
  locator.registerLazySingleton<PersonalInfoRepository>(
    () =>
        PersonalInfoRepositoryImpl(service: locator<PersonalInfoServiceMock>()),
  );
  locator.registerLazySingleton<ReviewsRepository>(
    () => ReviewsRepositoryImpl(service: locator<ReviewsServiceMock>()),
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
    () => GetContentByFilterUseCase(
      contentRepository: locator<ContentRepository>(),
    ),
  );
  locator.registerLazySingleton(
    () =>
        GetContentByIdUseCase(contentRepository: locator<ContentRepository>()),
  );
  locator.registerLazySingleton(
    () => GetSectionsUseCase(contentRepository: locator<ContentRepository>()),
  );
  locator.registerLazySingleton(
    () => GetAllGenresUseCase(genreRepository: locator<GenreRepository>()),
  );
  locator.registerLazySingleton(
    () => GetAllContentTypesUseCase(
      contentTypeRepository: locator<ContentTypeRepository>(),
    ),
  );
  locator.registerLazySingleton(
    () => GetFavoriteByFilterUseCase(
      favoriteRepository: locator<FavoriteRepository>(),
    ),
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
    () => ChangeBirthDateUseCase(repository: locator<PersonalInfoRepository>()),
  );
  locator.registerLazySingleton(
    () => ChangeEmailUseCase(repository: locator<PersonalInfoRepository>()),
  );
  locator.registerLazySingleton(
    () => ChangePasswordUseCase(repository: locator<PersonalInfoRepository>()),
  );
  locator.registerLazySingleton(
    () => GetUserInfoUseCase(repository: locator<PersonalInfoRepository>()),
  );

  locator.registerLazySingleton(
    () => GetReviewsUseCase(repository: locator<ReviewsRepository>()),
  );
  locator.registerLazySingleton(
    () => GetTotalReviewPagesUseCase(repository: locator<ReviewsRepository>()),
  );

  locator.registerLazySingleton(
    () => CancelSubscriptionUseCase(
      subscriptionRepository: locator<SubscriptionRepository>(),
      authRepository: locator<AuthRepository>(),
    ),
  );
}
