import 'package:netflix/domain/repositories/reviews_repository.dart';
import 'package:netflix/utils/result.dart';

class GetTotalReviewPagesUseCase {
  final ReviewsRepository repository;

  GetTotalReviewPagesUseCase({required this.repository});

  Future<Result<int>> call({String? search}) async {
    return repository.getTotalPages(search: search);
  }
}