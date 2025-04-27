import 'package:netflix/ui/auth/bloc/auth_state.dart';

abstract class AuthEvent {}

class AuthFieldsChanged extends AuthEvent {
  final String? login;
  final String? email;
  final String? password;

  AuthFieldsChanged({this.login, this.email, this.password});
}

class AuthFormTypeChanged extends AuthEvent {
  final AuthFormType formType;

  AuthFormTypeChanged(this.formType);
}