import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/utils/result.dart';

class ChangeEmailUseCase {
  final PersonalInfoRepository repository;

  ChangeEmailUseCase({required this.repository});

  Future<Result<UserInfo>> execute(String newEmail) async {
    try {
      final result = await repository.changeEmail(newEmail);
      return result;
    } catch (e) {
      return Result.error('Failed to change email: $e');
    }
  }
}