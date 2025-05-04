

import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/repositories/content_repository.dart';

import '../../utils/result.dart';

class GetContentByIdUseCase {
  final ContentRepository _contentRepository;

  GetContentByIdUseCase({required ContentRepository contentRepository})
      : _contentRepository = contentRepository;

  Future<Result<Content>> execute(int contentId) async {
    try {
      final result = await _contentRepository.getContentById(contentId: contentId);
      return result;
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}