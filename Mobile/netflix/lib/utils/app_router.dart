import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/ui/core/cubit/navigation/navigation_cubit.dart';
import 'package:netflix/ui/core/widgets/netflix_app_view.dart';
import 'package:netflix/ui/main/main_page.dart';
import 'package:netflix/ui/profile/profile_page.dart';
import 'package:netflix/ui/profile/tabs/favorites/favorites_view.dart';
import 'package:netflix/ui/profile/tabs/personal_info/personal_info_view.dart';
import 'package:netflix/ui/profile/tabs/reviews/reviews_view.dart';
import 'package:netflix/ui/profile/tabs/subscriptions/subscriptions_view.dart';
import 'package:netflix/ui/search/search_page.dart';
import 'package:netflix/utils/routes.dart';

class AppRouter {
  AppRouter._();
  
  static final _rootNavigatorKey = GlobalKey<NavigatorState>();
  static final _shellNavigatorKey = GlobalKey<NavigatorState>();

  static final GoRouter _router = GoRouter(
    initialLocation: Routes.main,
    debugLogDiagnostics: true,
    navigatorKey: _rootNavigatorKey,
    routes: [
      ShellRoute(
        navigatorKey: _shellNavigatorKey,
        builder: (context, state, child) {
          return BlocProvider(
            create: (context) => NavigationCubit(),
            child: NetflixAppView(screen: child),
          );
        },
        routes: [
          GoRoute(
            path: Routes.main,
            pageBuilder:
                (context, state) => const NoTransitionPage(child: MainPage()),
          ),
          GoRoute(
            path: Routes.search,
            pageBuilder:
                (context, state) => const NoTransitionPage(child: SearchPage()),
          ),
          GoRoute(
            path: Routes.profile,
            pageBuilder:
                (context, state) =>
                    const NoTransitionPage(child: ProfilePage()),
            routes: [
              GoRoute(
                path: Routes.profilePersonalRelative,
                builder: (context, state) => PersonalInfoView(),
              ),
              GoRoute(
                path: Routes.profileFavoritesRelative,
                builder: (context, state) => FavoritesView(),
              ),
              GoRoute(
                path: Routes.profileReviewsRelative,
                builder: (context, state) => ReviewsView(),
              ),
              GoRoute(
                path: Routes.profileSubscriptionsRelative,
                builder: (context, state) => SubscriptionsView(),
              ),
            ],
          ),
        ],
      ),
    ],
  );

  static GoRouter get router => _router;
}
