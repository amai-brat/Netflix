import 'dart:async';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';
import 'package:netflix/domain/use_cases/get_favorite_by_filter_use_case.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_event.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_state.dart';
import 'package:netflix/utils/result.dart';

class FavoriteBloc extends Bloc<FavoriteEvent, FavoriteState> {
  final GetFavoriteByFilterUseCase _getFavoriteByFilterUseCase;
  Timer? _searchTimer;

  FavoriteBloc({
    required GetFavoriteByFilterUseCase getFavoriteByFilterUseCase,
  })  : _getFavoriteByFilterUseCase = getFavoriteByFilterUseCase,
        super(FavoriteState(
          filterParams: const FavoriteFilterParams(),
      )) {
    on<LoadInitialData>(_onLoadInitialData);
    on<LoadFavorite>(_onLoadFavorite);
    on<SearchQueryChanged>(_onSearchQueryChanged);
    on<SortChanged>(_onSortChanged);
    on<UpdateFilterParams>(_onUpdateFilterParams);
    on<ApplyFilters>(_onApplyFilters);
    on<ResetFilters>(_onResetFilters);
    on<ResetSort>(_onResetSort);
  }

  Future<void> _onLoadInitialData(
      LoadInitialData event,
      Emitter<FavoriteState> emit,
      ) async {
    emit(state.copyWith(isLoading: true));

    await Future.delayed(Duration(milliseconds: 200));

    final favorites = await _getFavoriteByFilterUseCase.execute(state.filterParams, 0, state.perPage);

    switch (favorites) {
      case Ok(): {
        emit(state.copyWith(
          favorites: favorites.value,
          isLoading: false,
          isInit: true
        ));
        break;
      }
      default: {
        emit(state.copyWith(
          error: 'Ошибка загрузки данных',
          isLoading: false,
          isInit: false
        ));
        break;
      }
    }
  }

  Future<void> _onLoadFavorite(
      LoadFavorite event,
      Emitter<FavoriteState> emit,
      ) async {
    if (state.isLoadingMore || !state.hasMore || state.isLoading || !state.isInit) return;

    emit(state.copyWith(isLoadingMore: true));

    final nextPage = state.page + 1;

    await Future.delayed(Duration(milliseconds: 200));

    final result = await _getFavoriteByFilterUseCase.execute(
      state.filterParams, nextPage, state.perPage
    );

    switch (result) {
      case Ok():
        emit(state.copyWith(
          favorites: [...state.favorites, ...result.value],
          page: nextPage,
          hasMore: result.value.length >= state.perPage,
          isLoadingMore: false,
        ));
      default:
        emit(state.copyWith(
          error: 'Ошибка загрузки дополнительных данных',
          isLoadingMore: false,
        ));
    }
  }

  void _onSearchQueryChanged(
      SearchQueryChanged event,
      Emitter<FavoriteState> emit,
      ) {
    _searchTimer?.cancel();

    final newParams = state.filterParams.copyWith(
      searchQuery: (event.query, false),
    );

    emit(state.copyWith(filterParams: newParams));

    _searchTimer = Timer(const Duration(milliseconds: 300), () {
      add(ApplyFilters(newParams));
    });
  }

  void _onSortChanged(
      SortChanged event,
      Emitter<FavoriteState> emit,
      ){
    final newParams = state.filterParams.copyWith(
      sortBy: (event.sort, false),
    );
    emit(state.copyWith(
        filterParams: newParams
    ));
    add(ApplyFilters(newParams));
  }

  void _onUpdateFilterParams(
      UpdateFilterParams event,
      Emitter<FavoriteState> emit,
      ) {
    final newParams = event.updateFn(state.filterParams);
    emit(state.copyWith(
        filterParams: newParams
    ));
  }

  Future<void> _onApplyFilters(
      ApplyFilters event,
      Emitter<FavoriteState> emit,
      ) async {
    emit(state.copyWith(isLoading: true, page: 0, hasMore: true));
    final favorites = await _getFavoriteByFilterUseCase.execute(event.params, 0, state.perPage);

    switch(favorites){
      case Ok():{
        emit(state.copyWith(
          favorites: favorites.value,
          filterParams: state.filterParams,
          isLoading: false,
          page: 0,
          hasMore: favorites.value.length >= state.perPage
        ));
      }
      default:{
        emit(state.copyWith(
          error: 'Ошибка применения фильтров',
          isLoading: false,
        ));
      }
    }
  }

  void _onResetFilters(
      ResetFilters event,
      Emitter<FavoriteState> emit,
      ) {
    final resetParams = (const FavoriteFilterParams())
        .copyWith(sortBy: (state.filterParams.sortBy, false));

    emit(state.copyWith(
      filterParams: resetParams,
    ));
    add(ApplyFilters(resetParams));
  }

  void _onResetSort(
      ResetSort event,
      Emitter<FavoriteState> emit,
      ) {
    final resetParams = state.filterParams.copyWith(sortBy: (null, true));

    emit(state.copyWith(
      filterParams: resetParams,
    ));
    add(ApplyFilters(resetParams));
  }

  @override
  Future<void> close() {
    _searchTimer?.cancel();
    return super.close();
  }
}