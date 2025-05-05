part of 'reviews_bloc.dart';

sealed class ReviewsState {
  const ReviewsState();
}

class ReviewsLoading extends ReviewsState {
  const ReviewsLoading();
}

class ReviewsNormal extends ReviewsState {
  final List<UserReview> reviews;
  final int currentPage;
  final int totalPages;
  final bool isLoading;
  final String? search;
  final String? sort;

  const ReviewsNormal({
    required this.reviews,
    required this.currentPage,
    required this.totalPages,
    required this.isLoading,
    this.search,
    this.sort,
  });

  ReviewsNormal copyWith({
    List<UserReview>? reviews,
    int? currentPage,
    int? totalPages,
    bool? isLoading,
    String? search,
    String? sort,
  }) {
    return ReviewsNormal(
      reviews: reviews ?? this.reviews,
      currentPage: currentPage ?? this.currentPage,
      totalPages: totalPages ?? this.totalPages,
      isLoading: isLoading ?? this.isLoading,
      search: search ?? this.search,
      sort: sort ?? this.sort,
    );
  }
}

class ReviewsError extends ReviewsState {
  final String message;

  const ReviewsError(this.message);
}
