import 'package:netflix/data/models/api_profession_dto.dart';
import 'package:netflix/domain/models/content/person_in_content.dart';

class ApiPersonInContentDto {
  final int id;
  final int contentId;
  final String name;
  final ApiProfessionDto profession;

  const ApiPersonInContentDto({
    required this.id,
    required this.contentId,
    required this.name,
    required this.profession,
  });

  ApiPersonInContentDto.fromMap(Map<String, dynamic> map)
    : id = map['id'],
      contentId = map['contentId'],
      name = map['name'],
      profession = ApiProfessionDto.fromMap(map['profession']);

  PersonInContent toPersonInContent() => PersonInContent(
    id: id,
    contentId: contentId,
    name: name,
    profession: profession.toProfession(),
  );
}
