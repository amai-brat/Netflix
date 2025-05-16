class ApiSignUpDto {
  final int? id;

  ApiSignUpDto({required this.id});

  ApiSignUpDto.fromMap(Map<String, dynamic> map) : id = map['id'];
}
