import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/utils/result.dart';

abstract class ReviewsRepository {
  Future<Result<List<UserReview>>> getReviews({
    required int page,
    String? search,
    String? sort,
  });

  Future<Result<int>> getTotalPages({
    String? search,
  });
}