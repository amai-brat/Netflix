import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/utils/result.dart';

class ChangeBirthDateUseCase {
  final PersonalInfoRepository repository;

  ChangeBirthDateUseCase({required this.repository});

  Future<Result<UserInfo>> execute(String newBirthDate) async {
    try {
      final result = await repository.changeBirthDate(newBirthDate);
      return result;
    } catch (e) {
      return Result.error('Failed to change birth date: $e');
    }
  }
}