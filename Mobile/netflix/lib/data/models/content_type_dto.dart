import 'package:netflix/domain/models/content/content_type.dart';

class ContentTypeDto {
  final int id;
  final String name;

  const ContentTypeDto({required this.id, required this.name});

  ContentTypeDto.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        name = map['contentTypeName'];

  ContentType toContentType() => ContentType(id: id, name: name);
}