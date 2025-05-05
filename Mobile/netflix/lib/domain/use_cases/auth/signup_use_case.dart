import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/utils/result.dart';

class SignUpUseCase {
  final AuthRepository _authRepository;

  SignUpUseCase({required AuthRepository authRepository})
    : _authRepository = authRepository;

  Future<Result<void>> execute(
    String login,
    String email,
    String password,
  ) async {
    try {
      return await _authRepository.signUp(
        login: login,
        email: email,
        password: password,
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
