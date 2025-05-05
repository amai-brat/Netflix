import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/models/content/genre.dart';

class SearchState {
  final List<Content> contents;
  final int page;
  final int perPage;
  final bool hasMore;
  final bool isLoadingMore;
  final List<Genre> availableGenres;
  final List<ContentType> availableTypes;
  final List<String> availableCountries;
  final bool isLoading;
  final bool isInit;
  final String error;
  final ContentFilterParams filterParams;

  const SearchState({
    this.contents = const [],
    this.page = 0,
    this.perPage = 8,
    this.hasMore = true,
    this.isLoadingMore = false,
    this.availableGenres = const [],
    this.availableTypes = const [],
    /// Прикольно, что мы так и не реализовали репозиторий для стран на бэке (во фронте это просто хардкодом XD)
    this.availableCountries = const ['Не выбрано', 'Россия', 'США', 'Франция', 'Корея'],
    this.isLoading = false,
    this.isInit = false,
    this.error = '',
    required this.filterParams,
  });

  SearchState copyWith({
    List<Content>? contents,
    int? page,
    bool? hasMore,
    bool? isLoadingMore,
    List<Genre>? availableGenres,
    List<ContentType>? availableTypes,
    bool? isLoading,
    bool? isInit,
    String? error,
    ContentFilterParams? filterParams,
  }) {
    return SearchState(
      contents: contents ?? this.contents,
      page: page ?? this.page,
      perPage: perPage,
      hasMore: hasMore ?? this.hasMore,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      availableGenres: availableGenres ?? this.availableGenres,
      availableTypes: availableTypes ?? this.availableTypes,
      availableCountries: availableCountries,
      isLoading: isLoading ?? this.isLoading,
      isInit: isInit ?? this.isInit,
      error: error ?? this.error,
      filterParams: filterParams ?? this.filterParams,
    );
  }
}