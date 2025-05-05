import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/repositories/content_type_repository.dart';
import 'package:netflix/utils/result.dart';

class GetAllContentTypesUseCase {
  final ContentTypeRepository _contentTypeRepository;

  GetAllContentTypesUseCase({required ContentTypeRepository contentTypeRepository})
      : _contentTypeRepository = contentTypeRepository;

  Future<Result<List<ContentType>>> execute() async {
    try {
      return Result.ok(await _contentTypeRepository.getContentTypes());
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
