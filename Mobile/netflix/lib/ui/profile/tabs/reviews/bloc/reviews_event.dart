part of 'reviews_bloc.dart';

sealed class ReviewsEvent {}

class LoadReviewsEvent extends ReviewsEvent {
  final String? search;
  final String? sort;

  LoadReviewsEvent({this.search, this.sort});
}

class ChangePageEvent extends ReviewsEvent {
  final int page;

  ChangePageEvent(this.page);
}
