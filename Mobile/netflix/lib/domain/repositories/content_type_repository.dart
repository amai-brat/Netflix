import 'package:netflix/domain/models/content/content_type.dart';

abstract class ContentTypeRepository {
  Future<List<ContentType>> getContentTypes();
}