abstract class UserEvent {}

class AuthRequiredOpened extends UserEvent {}

class SignInPressed extends UserEvent {
  final String email;
  final String password;

  SignInPressed({required this.email, required this.password});
}

class SignUpPressed extends UserEvent {
  final String login;
  final String email;
  final String password;

  SignUpPressed({
    required this.login,
    required this.email,
    required this.password,
  });
}

class SignOutPressed extends UserEvent {}
