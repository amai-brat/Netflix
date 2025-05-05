import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/utils/result.dart';

class GetContentByFilterUseCase {
  final ContentRepository _contentRepository;

  GetContentByFilterUseCase({required ContentRepository contentRepository})
      : _contentRepository = contentRepository;

  Future<Result<List<Content>>> execute(ContentFilterParams params, int page, int perPage) async {
    try {
      return Result.ok(await _contentRepository.getContent(params, page, perPage));
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}