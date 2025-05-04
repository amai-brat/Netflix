import 'package:netflix/domain/models/content_type.dart';

abstract class ContentTypeRepository {
  Future<List<ContentType>> getContentTypes();
}