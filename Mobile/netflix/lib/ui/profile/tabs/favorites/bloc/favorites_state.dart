import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';

class FavoriteState {
  final List<Favorite> favorites;
  final int page;
  final int perPage;
  final bool hasMore;
  final bool isLoadingMore;
  final bool isLoading;
  final bool isInit;
  final String error;
  final FavoriteFilterParams filterParams;

  const FavoriteState({
    this.favorites = const [],
    this.page = 0,
    this.perPage = 4,
    this.hasMore = true,
    this.isLoadingMore = false,
    this.isLoading = false,
    this.isInit = false,
    this.error = '',
    required this.filterParams,
  });

  FavoriteState copyWith({
    List<Favorite>? favorites,
    int? page,
    bool? hasMore,
    bool? isLoadingMore,
    bool? isLoading,
    bool? isInit,
    String? error,
    FavoriteFilterParams? filterParams,
  }) {
    return FavoriteState(
      favorites: favorites ?? this.favorites,
      page: page ?? this.page,
      perPage: perPage,
      hasMore: hasMore ?? this.hasMore,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      isLoading: isLoading ?? this.isLoading,
      isInit: isInit ?? this.isInit,
      error: error ?? this.error,
      filterParams: filterParams ?? this.filterParams,
    );
  }
}