import 'package:netflix/domain/models/content_filter_params.dart';

abstract class SearchEvent {
  const SearchEvent();
}

class SearchQueryChanged extends SearchEvent {
  final String query;
  const SearchQueryChanged(this.query);
}

class SortChanged extends SearchEvent {
  final SortBy sort;
  const SortChanged(this.sort);
}

class LoadInitialData extends SearchEvent {}

class LoadContent extends SearchEvent {}

class UpdateFilterParams extends SearchEvent {
  final ContentFilterParams Function(ContentFilterParams currentParams) updateFn;
  const UpdateFilterParams(this.updateFn);
}

class ApplyFilters extends SearchEvent {
  final ContentFilterParams params;
  const ApplyFilters(this.params);
}

class ResetFilters extends SearchEvent {}

class ResetSort extends SearchEvent {}