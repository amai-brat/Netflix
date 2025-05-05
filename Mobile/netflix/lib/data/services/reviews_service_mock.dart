import 'dart:math';

import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/utils/result.dart';

class ReviewsServiceMock{
  Future<Result<List<UserReview>>> getReviews({
    required int page,
    String? search,
    String? sort,
  }) async {
    await Future.delayed(const Duration(milliseconds: 500));

    try {
      final reviews = List<UserReview>.generate(
        10,
        (index) => UserReview(
          id: index + page * 10,
          isPositive: Random().nextBool(),
          name: 'User ${index + page * 10}',
          score: Random().nextInt(5) + 1,
          text: 'Review text ${index + page * 10} ${search != null ? '(search: $search)' : ''}',
          contentName: 'Content ${Random().nextInt(10)}',
          writtenAt: DateTime.now().subtract(Duration(days: Random().nextInt(30))),
        ),
      );

      return Result.ok(reviews);
    } catch (e) {
      return Result.error('Failed to load reviews');
    }
  }

  Future<Result<int>> getTotalPages({String? search}) async {
    await Future.delayed(const Duration(milliseconds: 300));
    if (search != null && search.isNotEmpty){
      return Result.ok(5);
    }
    return Result.ok(9);
  }
}