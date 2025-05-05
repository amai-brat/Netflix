class UserInfo {
  final String nickname;
  final String birthDate;
  final String email;
  final String? profilePictureUrl;

  const UserInfo({
    required this.nickname,
    required this.birthDate,
    required this.email,
    this.profilePictureUrl,
  });
}