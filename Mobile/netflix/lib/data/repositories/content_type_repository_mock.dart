import 'package:netflix/domain/models/content_type.dart';
import 'package:netflix/domain/repositories/content_type_repository.dart';

class ContentTypeRepositoryMock extends ContentTypeRepository {

  @override
  Future<List<ContentType>> getContentTypes() async {
    await Future.delayed(Duration(milliseconds: 300));
    return [
      ContentType(id: 1, name: 'Фильм'),
      ContentType(id: 2, name: 'Сериал'),
      ContentType(id: 3, name: 'Мультфильм'),
    ];
  }
}