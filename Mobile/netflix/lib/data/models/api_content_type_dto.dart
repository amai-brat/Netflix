import '../../domain/models/content/content_type.dart';

class ApiContentTypeDto {
  final int id;
  final String contentTypeName;

  const ApiContentTypeDto({required this.id, required this.contentTypeName});

  ApiContentTypeDto.fromMap(Map<String, dynamic> map)
    : id = map['id'],
      contentTypeName = map['contentTypeName'];

  ContentType toContentType() => ContentType(id: id, name: contentTypeName);
}
