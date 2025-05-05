class FavoriteFilterParams {
  final String searchQuery;
  final FavoritesSortBy? sortBy;

  const FavoriteFilterParams({
    this.searchQuery = '',
    this.sortBy
  });

  FavoriteFilterParams copyWith({
    (String?, bool)? searchQuery,
    (FavoritesSortBy?, bool)? sortBy,
  }) {
    return FavoriteFilterParams(
        searchQuery: searchQuery == null ? this.searchQuery : searchQuery.$2 ? '' : searchQuery.$1 ?? this.searchQuery,
        sortBy: sortBy == null ? this.sortBy : sortBy.$2 ? sortBy.$1 : sortBy.$1 ?? this.sortBy,
    );
  }
}

enum FavoritesSortBy {
  userRatingDesc,
  userRatingAsc,
  addedDateDesc,
  addedDateAsc,
  publicRatingDesc,
  publicRatingAsc,
  dateDesc,
  dateAsc,
  titleAsc,
  titleDesc,
}