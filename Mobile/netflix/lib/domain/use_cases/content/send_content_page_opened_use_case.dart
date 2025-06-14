import 'package:flutter/foundation.dart';
import 'package:netflix/domain/repositories/content_repository.dart';

import '../../../utils/result.dart';

class SendContentPageOpenedUseCase {
  final ContentRepository _contentRepository;

  SendContentPageOpenedUseCase({required ContentRepository contentRepository})
      : _contentRepository = contentRepository;

  Future<Result<void>> execute(int contentId) async {
    try {
      await _contentRepository.sendContentPageOpened(contentId: contentId);
      return Result.ok(null);
    } catch (e) {
      debugPrint(e.toString());
      return Result.error(e.toString());
    }
  }
}