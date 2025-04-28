import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/use_cases/signin_use_case.dart';
import 'package:netflix/domain/use_cases/signout_use_case.dart';
import 'package:netflix/domain/use_cases/signup_use_case.dart';
import 'package:netflix/ui/core/bloc/user/user_bloc.dart';
import 'package:netflix/utils/app_router.dart';
import 'package:netflix/utils/app_theme.dart';
import 'package:netflix/utils/di.dart';

class NetflixApp extends StatelessWidget {
  const NetflixApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create:
              (context) => UserBloc(
                authRepository: locator<AuthRepository>(),
                signUpUseCase: locator<SignUpUseCase>(),
                signInUseCase: locator<SignInUseCase>(),
                signOutUseCase: locator<SignOutUseCase>(),
              ),
        ),
      ],
      child: MaterialApp.router(
        routerConfig: AppRouter.router,
        theme: AppTheme.theme,
      ),
    );
  }
}