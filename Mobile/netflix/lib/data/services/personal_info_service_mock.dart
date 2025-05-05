import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/utils/result.dart';

class PersonalInfoServiceMock {
  Future<Result<UserInfo>> getUserInfo() async {
    await Future.delayed(const Duration(seconds: 1));
    return Result.ok(const UserInfo(
      nickname: 'user123',
      birthDate: '01.01.1990',
      email: 'user@example.com'
    ));
  }

  Future<Result<UserInfo>> changeEmail(String newEmail) async {
    await Future.delayed(const Duration(seconds: 1));
    return Result.ok(UserInfo(
        nickname: 'user123',
        birthDate: '01.01.1990',
        email: newEmail
    ));
  }


  Future<Result<UserInfo>> changeBirthDate(String newBirthDate) async {
    await Future.delayed(const Duration(seconds: 1));
    return Result.ok(UserInfo(
        nickname: 'user123',
        birthDate: newBirthDate,
        email: 'user@example.com'
    ));
  }

  Future<Result<String>> changePassword({
    required String oldPassword,
    required String newPassword,
  }) async {
    await Future.delayed(const Duration(seconds: 1));

    if (oldPassword != 'correctPassword') {
      return Result.error('Неверный текущий пароль');
    }

    if (newPassword.length < 6) {
      return Result.error('Пароль должен содержать минимум 6 символов');
    }

    return Result.ok("user@example.com");
  }
}