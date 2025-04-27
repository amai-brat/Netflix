import 'package:netflix/domain/models/user.dart';

enum AuthFormType { signup, signin }

class AuthState {
  final AuthFormType formType;
  final String login;
  final String email;
  final String password;
  final String? error;

  AuthState({
    required this.formType,
    required this.login,
    required this.email,
    required this.password,
    this.error
  });

  AuthState.initial()
    : formType = AuthFormType.signup,
      login = '',
      email = '',
      password = '',
      error = null;

  AuthState copyWith({
    AuthFormType? formType,
    String? login,
    String? email,
    String? password,
    String? error,
    User? user
  }) {
    return AuthState(
      formType: formType ?? this.formType,
      login: login ?? this.login,
      email: email ?? this.email,
      password: password ?? this.password,
      error: error ?? this.error
    );
  }
}
