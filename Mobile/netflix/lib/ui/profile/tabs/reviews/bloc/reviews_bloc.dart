import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/domain/use_cases/user/get_reviews_use_case.dart';
import 'package:netflix/domain/use_cases/user/get_total_reviews_pages_use_case.dart';
import 'package:netflix/utils/di.dart';
import 'package:netflix/utils/result.dart';

part 'reviews_event.dart';
part 'reviews_state.dart';

class ReviewsBloc extends Bloc<ReviewsEvent, ReviewsState> {
  final GetReviewsUseCase getReviews;
  final GetTotalReviewPagesUseCase getTotalPages;

  ReviewsBloc({
    required this.getReviews,
    required this.getTotalPages,
  }) : super(const ReviewsLoading()) {
    on<LoadReviewsEvent>(_onLoadReviews);
    on<ChangePageEvent>(_onChangePage);
  }

  static ReviewsBloc createViaLocator() => ReviewsBloc(
    getReviews: locator<GetReviewsUseCase>(),
    getTotalPages: locator<GetTotalReviewPagesUseCase>(),
  );

  Future<void> _onLoadReviews(
      LoadReviewsEvent event,
      Emitter<ReviewsState> emit,
      ) async {
    emit(const ReviewsLoading());

    final totalPagesResult = await getTotalPages(search: event.search);

    if (totalPagesResult case Ok(value: final totalPages)) {
      final reviewsResult = await getReviews(
        page: 1,
        search: event.search,
        sort: event.sort,
      );

      if (reviewsResult case Ok(value: final reviews)) {
        emit(ReviewsNormal(
          reviews: reviews,
          currentPage: 1,
          totalPages: totalPages,
          isLoading: false,
          search: event.search,
          sort: event.sort,
        ));
      } else if (reviewsResult case Error(error: final error)) {
        emit(ReviewsError(error));
      }
    } else if (totalPagesResult case Error(error: final error)) {
      emit(ReviewsError(error));
    }
  }

  Future<void> _onChangePage(
      ChangePageEvent event,
      Emitter<ReviewsState> emit,
      ) async {
    final currentState = state;
    if (currentState is! ReviewsNormal) return;

    emit(currentState.copyWith(isLoading: true));

    final result = await getReviews(
      page: event.page,
      search: currentState.search,
      sort: currentState.sort,
    );

    if (result case Ok(value: final reviews)) {
      emit(currentState.copyWith(
        reviews: reviews,
        currentPage: event.page,
        isLoading: false,
      ));
    } else if (result case Error(error: final error)) {
      emit(ReviewsError(error));
    }
  }
}