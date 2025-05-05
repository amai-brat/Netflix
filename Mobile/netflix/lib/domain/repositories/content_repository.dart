import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/responses/sections_response.dart';
import 'package:netflix/utils/result.dart';

abstract class ContentRepository {
  Future<List<Content>> getContent(ContentFilterParams params, int page, int perPage);
  Future<Result<Content>> getContentById({required int contentId});
  Future<Result<SectionsResponse>> getSections();
}