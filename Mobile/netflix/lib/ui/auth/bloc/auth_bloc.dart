import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_event.dart';
import 'package:netflix/ui/auth/bloc/auth_state.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc() : super(AuthState.initial()) {
    on<AuthFieldsChanged>(_onAuthFieldsChanged);
    on<AuthFormTypeChanged>(_onAuthFormTypeChanged);
  }

  void _onAuthFieldsChanged(AuthFieldsChanged event, Emitter<AuthState> emit) {
    return emit(
      state.copyWith(
        login: event.login,
        email: event.email,
        password: event.password,
      ),
    );
  }

  void _onAuthFormTypeChanged(
    AuthFormTypeChanged event,
    Emitter<AuthState> emit,
  ) {
    emit(state.copyWith(formType: event.formType));
  }
}
