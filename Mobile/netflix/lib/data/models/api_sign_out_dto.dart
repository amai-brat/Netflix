class ApiSignOutDto {
  final int code;
  final String? message;

  ApiSignOutDto({required this.code, required this.message});

  ApiSignOutDto.fromMap(Map<String, dynamic> map)
      : code = map['code'],
        message = map['message'];
}