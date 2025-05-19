import 'package:netflix/domain/models/favorite_filter_params.dart';

abstract class FavoriteEvent {
  const FavoriteEvent();
}

class SearchQueryChanged extends FavoriteEvent {
  final String query;
  const SearchQueryChanged(this.query);
}

class SortChanged extends FavoriteEvent {
  final FavoritesSortBy sort;
  const SortChanged(this.sort);
}

class LoadInitialData extends FavoriteEvent {}

class LoadFavorite extends FavoriteEvent {}

class UpdateFilterParams extends FavoriteEvent {
  final FavoriteFilterParams Function(FavoriteFilterParams currentParams) updateFn;
  const UpdateFilterParams(this.updateFn);
}

class ApplyFilters extends FavoriteEvent {
  final FavoriteFilterParams params;
  const ApplyFilters(this.params);
}

class ResetFilters extends FavoriteEvent {}

class ResetSort extends FavoriteEvent {}

class RemoveFavorite extends FavoriteEvent {
  final int contentId;
  const RemoveFavorite(this.contentId);
}