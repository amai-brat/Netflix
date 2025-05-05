import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/responses/sections_response.dart';
import 'package:netflix/domain/use_cases/content/get_sections_use_case.dart';
import 'package:netflix/ui/main/bloc/main_page_event.dart';
import 'package:netflix/ui/main/bloc/main_page_state.dart';
import 'package:netflix/utils/result.dart';

import '../../../utils/di.dart';

class MainPageBloc extends Bloc<MainPageEvent, MainPageState> {
  final GetSectionsUseCase _getSectionsUseCase;

  MainPageBloc({required GetSectionsUseCase getSectionsUseCase})
    : _getSectionsUseCase = getSectionsUseCase,
      super(MainPageState.initial()) {
    on<MainPageOpened>(_onMainPageOpened);
  }

  static MainPageBloc createViaLocator() {
    return MainPageBloc(getSectionsUseCase: locator<GetSectionsUseCase>());
  }

  FutureOr<void> _onMainPageOpened(
    MainPageOpened event,
    Emitter<MainPageState> emit,
  ) async {
    emit(state.copyWith(status: MainPageStatus.loading));

    final result = await _getSectionsUseCase.execute();
    switch (result) {
      case Ok<SectionsResponse>():
        emit(
          state.copyWith(
            status: MainPageStatus.completed,
            sections: result.value.data,
          ),
        );
      case Error<SectionsResponse>():
        emit(
          state.copyWith(status: MainPageStatus.completed, error: result.error),
        );
    }
  }
}
