import 'package:netflix/domain/models/content/content_type.dart';
import 'content/genre.dart';

class ContentFilterParams {
  final String searchQuery;
  final SortBy? sortBy;
  final List<Genre> selectedGenres;
  final List<ContentType> selectedTypes;
  final String? country;
  final int? yearFrom;
  final int? yearTo;
  final double? ratingFrom;
  final double? ratingTo;

  const ContentFilterParams({
    this.searchQuery = '',
    this.sortBy,
    this.selectedGenres = const [],
    this.selectedTypes = const [],
    this.country,
    this.yearFrom,
    this.yearTo,
    this.ratingFrom,
    this.ratingTo,
  });

  ContentFilterParams copyWith({
    (String?, bool)? searchQuery,
    (SortBy?, bool)? sortBy,
    (List<Genre>?, bool)? selectedGenres,
    (List<ContentType>?, bool)? selectedTypes,
    (String?, bool)? country,
    (int?, bool)? yearFrom,
    (int?, bool)? yearTo,
    (double?, bool)? ratingFrom,
    (double?, bool)? ratingTo
  }) {
    return ContentFilterParams(
      searchQuery: searchQuery == null ? this.searchQuery : searchQuery.$2 ? '' : searchQuery.$1 ?? this.searchQuery,
      sortBy: sortBy == null ? this.sortBy : sortBy.$2 ? sortBy.$1 : sortBy.$1 ?? this.sortBy,
      selectedGenres: selectedGenres == null ? this.selectedGenres : selectedGenres.$2 ? [] : selectedGenres.$1 ?? this.selectedGenres,
      selectedTypes: selectedTypes == null ? this.selectedTypes : selectedTypes.$2 ? [] : selectedTypes.$1 ?? this.selectedTypes,
      country: country == null ? this.country : country.$2 ? null : country.$1 ?? this.country,
      yearFrom: yearFrom == null ? this.yearFrom : yearFrom.$2 ? yearFrom.$1 : yearFrom.$1 ?? this.yearFrom,
      yearTo: yearTo == null ? this.yearTo : yearTo.$2 ? yearTo.$1 : yearTo.$1 ?? this.yearTo,
      ratingFrom: ratingFrom == null ? this.ratingFrom : ratingFrom.$2 ? ratingFrom.$1 : ratingFrom.$1 ?? this.ratingFrom,
      ratingTo: ratingTo == null ? this.ratingTo : ratingTo.$2 ? ratingTo.$1 : ratingTo.$1 ?? this.ratingTo
    );
  }
}

enum SortBy {
  ratingDesc,
  ratingAsc,
  dateDesc,
  dateAsc,
  titleAsc,
  titleDesc,
}