import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/domain/repositories/reviews_repository.dart';
import 'package:netflix/utils/result.dart';

class GetReviewsUseCase {
  final ReviewsRepository repository;

  GetReviewsUseCase({required this.repository});

  Future<Result<List<UserReview>>> call({
    required int page,
    String? search,
    String? sort,
  }) async {
    return repository.getReviews(
      page: page,
      search: search,
      sort: sort,
    );
  }
}