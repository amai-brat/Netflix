import 'package:netflix/data/services/reviews_service.dart';
import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/domain/repositories/reviews_repository.dart';
import 'package:netflix/utils/result.dart';

class ReviewsRepositoryImpl implements ReviewsRepository {
  final ReviewsService service;

  ReviewsRepositoryImpl({required this.service});

  @override
  Future<Result<List<UserReview>>> getReviews({
    required int page,
    String? search,
    String? sort,
  }) async {
    return service.getReviews(
      page: page,
      search: search,
      sort: sort,
    );
  }

  @override
  Future<Result<int>> getTotalPages({String? search}) {
    return service.getTotalPages(search: search);
  }
}