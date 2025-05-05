import 'package:netflix/utils/result.dart';

abstract class AuthRepository {
  Future<Result<void>> signUp({
    required String login,
    required String email,
    required String password,
  });

  Future<Result<void>> signIn({
    required String email,
    required String password,
  });

  Future<Result<void>> signOut();

  Future<bool> get isAuthenticated;
  Future<int?> get currentUserId;
}
