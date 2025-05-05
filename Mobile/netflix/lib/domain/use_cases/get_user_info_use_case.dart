import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/utils/result.dart';

class GetUserInfoUseCase {
  final PersonalInfoRepository repository;

  GetUserInfoUseCase({required this.repository});

  Future<Result<UserInfo>> execute() async {
    try {
      final result = await repository.getUserInfo();
      return result;
    } catch (e) {
      return Result.error('Failed to load user info: $e');
    }
  }
}