import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/utils/result.dart';

class SignOutUseCase {
  final AuthRepository _authRepository;

  SignOutUseCase({required AuthRepository authRepository})
    : _authRepository = authRepository;

  Future<Result<void>> execute() async {
    try {
      return await _authRepository.signOut();
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
