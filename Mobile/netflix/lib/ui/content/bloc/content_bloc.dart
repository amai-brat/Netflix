import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/use_cases/get_content_by_id_use_case.dart';
import 'package:netflix/ui/content/bloc/content_event.dart';
import 'package:netflix/ui/content/bloc/content_state.dart';
import 'package:netflix/utils/result.dart';

import '../../../utils/di.dart';

class ContentBloc extends Bloc<ContentEvent, ContentState> {
  final GetContentByIdUseCase _getContentByIdUseCase;

  ContentBloc({required GetContentByIdUseCase getContentByIdUseCase})
    : _getContentByIdUseCase = getContentByIdUseCase,
      super(ContentState.initial()) {
    on<ContentPageOpened>(_onContentPageOpened);
  }

  static ContentBloc createViaLocator() {
    return ContentBloc(getContentByIdUseCase: locator<GetContentByIdUseCase>());
  }

  FutureOr<void> _onContentPageOpened(
    ContentPageOpened event,
    Emitter<ContentState> emit,
  ) async {
    emit(state.copyWith(isLoading: true));

    final result = await _getContentByIdUseCase.execute(event.contentId);
    switch (result) {
      case Ok<Content>():
        emit(state.copyWith(isLoading: false, content: () => result.value));
      case Error<Content>():
        emit(state.copyWith(isLoading: false, error: result.error));
    }
  }
}
