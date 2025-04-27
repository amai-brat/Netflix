import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/use_cases/signin_use_case.dart';
import 'package:netflix/domain/use_cases/signout_use_case.dart';
import 'package:netflix/domain/use_cases/signup_use_case.dart';
import 'package:netflix/ui/core/bloc/user/user_event.dart';
import 'package:netflix/ui/core/bloc/user/user_state.dart';
import 'package:netflix/utils/result.dart';

class UserBloc extends Bloc<UserEvent, UserState> {
  UserBloc({
    required AuthRepository authRepository,
    required SignUpUseCase signUpUseCase,
    required SignInUseCase signInUseCase,
    required SignOutUseCase signOutUseCase,
  }) : _authRepository = authRepository,
       _signUpUseCase = signUpUseCase,
       _signInUseCase = signInUseCase,
       _signOutUseCase = signOutUseCase,
       super(UserState.initial()) {
    on<AuthRequiredOpened>(_onAuthRequiredOpened);
    on<SignUpPressed>(_onSignUpPressed);
    on<SignInPressed>(_onSignInPressed);
    on<SignOutPressed>(_onSignOutPressed);
  }

  final AuthRepository _authRepository;
  final SignUpUseCase _signUpUseCase;
  final SignInUseCase _signInUseCase;
  final SignOutUseCase _signOutUseCase;

  FutureOr<void> _onAuthRequiredOpened(
    AuthRequiredOpened event,
    Emitter<UserState> emit,
  ) async {
    emit(state.copyWith(status: UserStatus.loading));
    final isAuthenticated = await _authRepository.isAuthenticated;
    emit(
      state.copyWith(
        status:
            isAuthenticated
                ? UserStatus.authenticated
                : UserStatus.unauthenticated,
        isAuthenticated: isAuthenticated,
      ),
    );
  }

  FutureOr<void> _onSignUpPressed(SignUpPressed event, Emitter<UserState> emit) async {
    final resultUp = await _signUpUseCase.execute(
      event.login,
      event.email,
      event.password,
    );
    switch (resultUp) {
      case Error<void>():
        emit(state.copyWith(error: resultUp.error));
        return;
      case Ok<void>():
        break;
    }
  }

  FutureOr<void> _onSignInPressed(SignInPressed event, Emitter<UserState> emit) async {
    final resultIn = await _signInUseCase.execute(event.email, event.password);
    switch (resultIn) {
      case Ok<void>():
        emit(state.copyWith(status: UserStatus.authenticated, isAuthenticated: true));
      case Error<void>():
        emit(state.copyWith(error: resultIn.error));
    }
  }

  FutureOr<void> _onSignOutPressed(SignOutPressed event, Emitter<UserState> emit) async {
    final resultOut = await _signOutUseCase.execute();
    switch (resultOut) {
      case Ok<void>():
        emit(state.copyWith(status: UserStatus.unauthenticated, isAuthenticated: false));
      case Error<void>():
        emit(state.copyWith(error: resultOut.error));
    }
  }
}
