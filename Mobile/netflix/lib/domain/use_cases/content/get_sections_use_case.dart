import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/responses/sections_response.dart';

import '../../../utils/result.dart';

class GetSectionsUseCase {
  final ContentRepository _contentRepository;

  GetSectionsUseCase({required ContentRepository contentRepository})
    : _contentRepository = contentRepository;

  Future<Result<SectionsResponse>> execute() async {
    try {
      final result = await _contentRepository.getSections();
      return result;
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
