class ApiSignInDto {
  final int code;
  final String? message;
  final ApiTokensDto? tokens;

  ApiSignInDto({
    required this.code,
    required this.message,
    required this.tokens,
  });

  ApiSignInDto.fromMap(Map<String, dynamic> map)
    : code = map['code'],
      message = map['message'],
      tokens = ApiTokensDto.fromMap(map['tokens']);
}

class ApiTokensDto {
  final String accessToken;
  final String? refreshToken;

  ApiTokensDto({required this.accessToken, required this.refreshToken});

  ApiTokensDto.fromMap(Map<String, dynamic> map)
    : accessToken = map['accessToken'],
      refreshToken = map['refreshToken'];
}
