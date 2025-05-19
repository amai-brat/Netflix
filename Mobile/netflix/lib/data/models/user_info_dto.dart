import 'package:netflix/domain/models/user_info.dart';

class UserInfoDto {
  final String nickname;
  final String? birthDate;
  final String email;
  final String? profilePictureUrl;

  UserInfoDto({
    required this.nickname,
    required this.birthDate,
    required this.email,
    this.profilePictureUrl,
  });

  factory UserInfoDto.fromMap(Map<String, dynamic> map) {
    return UserInfoDto(
      nickname: map['nickname'] as String,
      birthDate: map['birthDay'] as String?,
      email: map['email'] as String,
      profilePictureUrl: map['profilePictureUrl'] as String?,
    );
  }

  UserInfo toUserInfo() {
    return UserInfo(
      nickname: nickname,
      birthDate: birthDate ?? '',
      email: email,
      profilePictureUrl: profilePictureUrl,
    );
  }
}