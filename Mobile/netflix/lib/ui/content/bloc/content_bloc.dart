import 'dart:async';

import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/use_cases/content/get_content_by_id_use_case.dart';
import 'package:netflix/domain/use_cases/content/send_content_page_opened_use_case.dart';
import 'package:netflix/ui/content/bloc/content_event.dart';
import 'package:netflix/ui/content/bloc/content_state.dart';
import 'package:netflix/utils/result.dart';

import '../../../utils/di.dart';

class ContentBloc extends Bloc<ContentEvent, ContentState> {
  final GetContentByIdUseCase _getContentByIdUseCase;
  final SendContentPageOpenedUseCase _sendContentPageOpenedUseCase;
  final ContentRepository _contentRepository;

  ContentBloc({
    required GetContentByIdUseCase getContentByIdUseCase,
    required SendContentPageOpenedUseCase sendContentPageOpenedUseCase,
    required ContentRepository contentRepository,
  }) : _getContentByIdUseCase = getContentByIdUseCase,
       _sendContentPageOpenedUseCase = sendContentPageOpenedUseCase,
       _contentRepository = contentRepository,
       super(ContentState.initial()) {
    on<ContentPageOpened>(_onContentPageOpened);
  }

  factory ContentBloc.createViaLocator() {
    return ContentBloc(
      getContentByIdUseCase: locator<GetContentByIdUseCase>(),
      sendContentPageOpenedUseCase: locator<SendContentPageOpenedUseCase>(),
      contentRepository: locator<ContentRepository>(),
    );
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

    await _sendContentPageOpenedUseCase.execute(event.contentId);
    await emit.forEach(
      await _contentRepository.getContentViewsById(contentId: event.contentId),
      onData: (data) => state.copyWith(contentViews: data, error: ''),
    );
  }

  @override
  Future<void> close() {
    _contentRepository.stopContentViewsStream();
    return super.close();
  }
}
