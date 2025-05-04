import 'dart:async';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/use_cases/get_all_content_types_use_case.dart';
import 'package:netflix/domain/use_cases/get_all_genres_use_case.dart';
import 'package:netflix/domain/use_cases/get_content_by_filter_use_case.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/result.dart';

class SearchBloc extends Bloc<SearchEvent, SearchState> {
  final GetContentByFilterUseCase _getContentByFilterUseCase;
  final GetAllGenresUseCase _getAllGenresUseCase;
  final GetAllContentTypesUseCase _getAllContentTypesUseCase;
  Timer? _searchTimer;

  SearchBloc({
    required GetContentByFilterUseCase getContentByFilterUseCase,
    required GetAllGenresUseCase getAllGenresUseCase,
    required GetAllContentTypesUseCase getAllContentTypesUseCase,
  })  : _getContentByFilterUseCase = getContentByFilterUseCase,
        _getAllGenresUseCase = getAllGenresUseCase,
        _getAllContentTypesUseCase = getAllContentTypesUseCase,
        super(SearchState(
        filterParams: const ContentFilterParams(),
      )) {
    on<LoadInitialData>(_onLoadInitialData);
    on<LoadContent>(_onLoadContent);
    on<SearchQueryChanged>(_onSearchQueryChanged);
    on<SortChanged>(_onSortChanged);
    on<UpdateFilterParams>(_onUpdateFilterParams);
    on<ApplyFilters>(_onApplyFilters);
    on<ResetFilters>(_onResetFilters);
    on<ResetSort>(_onResetSort);
  }

  Future<void> _onLoadInitialData(
      LoadInitialData event,
      Emitter<SearchState> emit,
      ) async {
    emit(state.copyWith(isLoading: true));

    await Future.delayed(Duration(milliseconds: 200));

    final contents = await _getContentByFilterUseCase.execute(state.filterParams, 0, state.perPage);
    final genres = await _getAllGenresUseCase.execute();
    final types = await _getAllContentTypesUseCase.execute();
    final result = (contents, genres, types);

    switch (result) {
      case (Ok(), Ok(), Ok()): {
        emit(state.copyWith(
          contents: result.$1.value,
          availableGenres: result.$2.value,
          availableTypes: result.$3.value,
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

  Future<void> _onLoadContent(
      LoadContent event,
      Emitter<SearchState> emit,
      ) async {
    if (state.isLoadingMore || !state.hasMore || state.isLoading || !state.isInit) return;

    emit(state.copyWith(isLoadingMore: true));

    final nextPage = state.page + 1;

    await Future.delayed(Duration(milliseconds: 200));

    final result = await _getContentByFilterUseCase.execute(
      state.filterParams, nextPage, state.perPage
    );

    switch (result) {
      case Ok():
        emit(state.copyWith(
          contents: [...state.contents, ...result.value],
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
      Emitter<SearchState> emit,
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
      Emitter<SearchState> emit,
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
      Emitter<SearchState> emit,
      ) {
    final newParams = event.updateFn(state.filterParams);
    emit(state.copyWith(
        filterParams: newParams
    ));
  }

  Future<void> _onApplyFilters(
      ApplyFilters event,
      Emitter<SearchState> emit,
      ) async {
    emit(state.copyWith(isLoading: true, page: 0, hasMore: true));
    final contents = await _getContentByFilterUseCase.execute(event.params, 0, state.perPage);

    switch(contents){
      case Ok():{
        emit(state.copyWith(
          contents: contents.value,
          filterParams: state.filterParams,
          isLoading: false,
          page: 0,
          hasMore: contents.value.length >= state.perPage
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
      Emitter<SearchState> emit,
      ) {
    final resetParams = (const ContentFilterParams())
        .copyWith(sortBy: (state.filterParams.sortBy, false));

    emit(state.copyWith(
      filterParams: resetParams,
    ));
    add(ApplyFilters(resetParams));
  }

  void _onResetSort(
      ResetSort event,
      Emitter<SearchState> emit,
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