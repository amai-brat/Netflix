import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_state.dart';

class SupportChatControlBloc extends Bloc<SupportChatControlEvent, SupportChatControlState> {
  SupportChatControlBloc() :
        super(SupportChatControlConnected(messageText: '', pickedFiles: null)) {
    on<MessageTextChangedEvent>(_onMessageTextChanged);
    on<FilesSelectedEvent>(_onFilesSelected);
    on<FileRemovedEvent>(_onFileRemoved);
    on<ResetEvent>(_onReset);
  }

  Future<void> _onMessageTextChanged(
      MessageTextChangedEvent event,
      Emitter<SupportChatControlState> emit,
      ) async {
    if (state is! SupportChatControlConnected) {
      return;
    }
    emit((state as SupportChatControlConnected).copyWith(messageText: event.text));
  }

  Future<void> _onFilesSelected(
      FilesSelectedEvent event,
      Emitter<SupportChatControlState> emit,
      ) async {
    if (state is! SupportChatControlConnected) {
      return;
    }
    final connectedState = state as SupportChatControlConnected;
    emit(connectedState.copyWith(pickedFiles: ([...connectedState.pickedFiles ?? [],...event.files], false)));
  }

  Future<void> _onFileRemoved(
      FileRemovedEvent event,
      Emitter<SupportChatControlState> emit,
      ) async {
    if (state is! SupportChatControlConnected) {
      return;
    }
    final connectedState = state as SupportChatControlConnected;
    emit(connectedState.copyWith(pickedFiles: (connectedState.pickedFiles?.where((file) => file != event.file).toList(), false)));
  }

  Future<void> _onReset(
      ResetEvent event,
      Emitter<SupportChatControlState> emit
      ) async {
    if (state is! SupportChatControlConnected) {
      return;
    }
    final connectedState = state as SupportChatControlConnected;
    emit(connectedState.copyWith(messageText: '', pickedFiles: (null, true)));
  }
}