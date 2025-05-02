import 'package:netflix/domain/models/content.dart';
import 'package:netflix/domain/models/content_filter_params.dart';

abstract class ContentRepository {
  ///
  Future<List<Content>> getContent(ContentFilterParams params, int page, int perPage);
}