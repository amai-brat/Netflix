import 'package:netflix/domain/models/sections/section.dart';
import 'package:netflix/domain/models/sections/section_content.dart';

class ApiSectionContentDto {
  final int id;
  final String name;
  final String posterUrl;

  ApiSectionContentDto({
    required this.id,
    required this.name,
    required this.posterUrl,
  });

  ApiSectionContentDto.fromMap(Map<String, dynamic> map)
    : id = map['id'],
      name = map['name'],
      posterUrl = map['posterUrl'];

  SectionContent toSectionContent() =>
      SectionContent(id: id, name: name, posterUrl: posterUrl);
}

class ApiSectionDto {
  final String name;
  final List<ApiSectionContentDto> contents;

  ApiSectionDto({required this.name, required this.contents});

  ApiSectionDto.fromMap(Map<String, dynamic> map)
    : name = map['name'],
      contents =
          (map['contents'] as List)
              .map((c) => ApiSectionContentDto.fromMap(c))
              .toList();

  Section toSection() => Section(
    name: name,
    contents: contents.map((c) => c.toSectionContent()).toList(),
  );
}
