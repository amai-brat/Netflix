import 'package:netflix/domain/models/content/profession.dart';

class ApiProfessionDto {
  final int id;
  final String professionName;

  const ApiProfessionDto({required this.id, required this.professionName});

  ApiProfessionDto.fromMap(Map<String, dynamic> map)
    : id = map['id'],
      professionName = map['professionName'];

  Profession toProfession() =>
      Profession(id: id, professionName: professionName);
}
