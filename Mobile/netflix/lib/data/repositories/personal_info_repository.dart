import 'package:netflix/data/services/personal_info_service.dart';
import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/domain/repositories/personal_info_repository.dart';
import 'package:netflix/utils/result.dart';

class PersonalInfoRepositoryImpl implements PersonalInfoRepository {
  final PersonalInfoService service;

  PersonalInfoRepositoryImpl({required this.service});

  @override
  Future<Result<UserInfo>> getUserInfo() => service.getUserInfo();

  @override
  Future<Result<UserInfo>> changeEmail(String newEmail) =>
      service.changeEmail(newEmail);

  @override
  Future<Result<UserInfo>> changeBirthDate(String newBirthDate) =>
      service.changeBirthDate(newBirthDate);

  @override
  Future<Result<String>> changePassword({
    required String oldPassword,
    required String newPassword,
  }) => service.changePassword(
    oldPassword: oldPassword,
    newPassword: newPassword,
  );
}