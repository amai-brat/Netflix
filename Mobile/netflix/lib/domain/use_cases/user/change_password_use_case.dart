import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/utils/result.dart';

class ChangePasswordUseCase {
  final PersonalInfoRepository repository;

  ChangePasswordUseCase({required this.repository});

  Future<Result<String>> execute({
    required String oldPassword,
    required String newPassword,
  }) async {
    try {
      final result = await repository.changePassword(
        oldPassword: oldPassword,
        newPassword: newPassword,
      );
      return result;
    } catch (e) {
      return Result.error('Failed to change password: $e');
    }
  }
}