import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/utils/result.dart';

abstract class PersonalInfoRepository {
  Future<Result<UserInfo>> getUserInfo();
  Future<Result<UserInfo>> changeEmail(String newEmail);
  Future<Result<UserInfo>> changeBirthDate(String newBirthDate);
  Future<Result<String>> changePassword({
    required String oldPassword,
    required String newPassword,
  });
}