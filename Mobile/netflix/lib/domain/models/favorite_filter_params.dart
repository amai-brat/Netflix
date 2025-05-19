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
  titleDesc;

  String get stringValue => switch (this) {
    FavoritesSortBy.addedDateDesc => 'ADDED_DATE_DESC',
    FavoritesSortBy.addedDateAsc => 'ADDED_DATE_ASC',
    FavoritesSortBy.userRatingDesc => 'USER_RATING_DESC',
    FavoritesSortBy.userRatingAsc => 'USER_RATING_ASC',
    FavoritesSortBy.publicRatingDesc => 'PUBLIC_RATING_DESC',
    FavoritesSortBy.publicRatingAsc => 'PUBLIC_RATING_ASC',
    FavoritesSortBy.dateDesc => 'DATE_DESC',
    FavoritesSortBy.dateAsc => 'DATE_ASC',
    FavoritesSortBy.titleAsc => 'TITLE_ASC',
    FavoritesSortBy.titleDesc => 'TITLE_DESC',
  };
}