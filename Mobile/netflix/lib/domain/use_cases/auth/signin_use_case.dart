import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/utils/result.dart';

class SignInUseCase {
  final AuthRepository _authRepository;

  SignInUseCase({required AuthRepository authRepository})
    : _authRepository = authRepository;

  Future<Result<void>> execute(String email, String password) async {
    try {
      return await _authRepository.signIn(email: email, password: password);
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
