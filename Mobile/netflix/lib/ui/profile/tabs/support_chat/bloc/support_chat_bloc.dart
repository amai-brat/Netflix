import 'dart:async';
import 'package:file_picker/file_picker.dart';
import 'package:fixnum/fixnum.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:netflix/clients/grpc_support_chat_client.dart';
import 'package:netflix/domain/use_cases/support/get_history_use_case.dart';
import 'package:netflix/domain/use_cases/support/upload_files_use_case.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/utils/consts.dart';
import 'package:netflix/utils/di.dart';
import 'package:netflix/utils/jwt_decoder.dart';
import 'package:netflix/utils/result.dart';

class SupportChatBloc extends Bloc<SupportChatEvent, SupportChatState> {
  final GrpcSupportChatClient _client;
  final UploadFilesUseCase _uploadFilesUseCase;
  final GetHistoryUseCase _getHistoryUseCase;

  StreamSubscription<SupportChatMessage>? _streamSubscription;
  String? _sessionId;
  String? _role;
  Int64? _userId;

  SupportChatBloc({
    required GrpcSupportChatClient client,
    required UploadFilesUseCase uploadFilesUseCase,
    required GetHistoryUseCase getHistoryUseCase,
  }) :  _client = client,
        _uploadFilesUseCase = uploadFilesUseCase,
        _getHistoryUseCase = getHistoryUseCase,
        super(SupportChatInitial()) {
    on<ConnectSupportChatEvent>(_onConnect);
    on<MessageTextChangedEvent>(_onMessageTextChanged);
    on<FilesSelectedEvent>(_onFilesSelected);
    on<FileRemovedEvent>(_onFileRemoved);
    on<SendMessageEvent>(_onSendMessage);
    on<DisconnectSupportChatEvent>(_onDisconnect);
  }

  Future<void> _onConnect(
      ConnectSupportChatEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    try {
      emit(SupportChatLoading());
      await _setUserData();
      final metadata = await _getMetadata();
      final response = await _client.connect(metadata);
      _sessionId = response.sessionId;

      _streamSubscription = _client
          .connectToStream(_sessionId!, metadata)
          .listen((message) {_handleIncomingMessage(message, emit);});

      emit(SupportChatConnected());
      await _loadHistory(emit);
    } catch (e) {
      emit(SupportChatError('Не удалось подключиться'));
    }
  }

  Future<void> _onMessageTextChanged(
      MessageTextChangedEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    if (state is! SupportChatConnected) {
      emit(SupportChatError('Отправка сообщений недоступна'));
      return;
    }
    emit((state as SupportChatConnected).copyWith(messageText: event.text));
  }

  Future<void> _onFilesSelected(
      FilesSelectedEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    if (state is! SupportChatConnected) {
      emit(SupportChatError('Отправка сообщений недоступна'));
      return;
    }
    final connectedState = state as SupportChatConnected;
    emit(connectedState.copyWith(pickedFiles: ([...connectedState.pickedFiles ?? [],...event.files], false)));
  }

  Future<void> _onFileRemoved(
      FileRemovedEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    if (state is! SupportChatConnected) {
      emit(SupportChatError('Отправка сообщений недоступна'));
      return;
    }
    final connectedState = state as SupportChatConnected;
    emit(connectedState.copyWith(pickedFiles: (connectedState.pickedFiles?.where((file) => file != event.file).toList(), false)));
  }


  Future<void> _onSendMessage(
      SendMessageEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    if (state is! SupportChatConnected) {
      emit(SupportChatError('Не удалось отправить сообщение'));
      return;
    }

    try {
      final connectedState = state as SupportChatConnected;
      final metadata = await _getMetadata();
      final uploadedFiles = await _uploadFiles(connectedState.pickedFiles);

      await _client.sendMessage(_userId!, _sessionId!, connectedState.messageText, uploadedFiles, metadata);

      final newMessage = SupportChatMessageBase(
        text: connectedState.messageText,
        files: uploadedFiles,
        role: _role,
      );

      emit(connectedState.copyWith(
        messageText: '',
        pickedFiles: (null, true),
        messages: [...connectedState.messages, newMessage]
      ));
    } catch (e) {
      emit(SupportChatError('Не удалось отправить сообщение'));
    }
  }

  Future<void> _onDisconnect(
      DisconnectSupportChatEvent event,
      Emitter<SupportChatState> emit,
      ) async {
    await _streamSubscription?.cancel();
    if (_sessionId != null) {
      await _client.disconnect(_sessionId!, await _getMetadata());
    }
    emit(SupportChatInitial());
  }

  Future<Map<String, String>> _getMetadata() async {
    final token = await locator<FlutterSecureStorage>().read(key: Consts.accessToken);
    return {'authorization': 'Bearer $token'};
  }

  Future<void> _setUserData() async {
    final token = await locator<FlutterSecureStorage>().read(key: Consts.accessToken);
    if(token != null)
    {
      final userId = Int64.parseInt(JwtDecoder.decode(token)['id']);
      _userId = userId;
      _role = 'user';
    }
  }

  Future<void> _loadHistory(Emitter<SupportChatState> emit) async {
    final history = await _getHistoryUseCase.execute();
    switch(history){
      case Ok():
        final connectedState = state as SupportChatConnected;
        emit(connectedState.copyWith(messages: [...connectedState.messages, ...history.value]));
        break;
      default:
        emit(SupportChatError('Не удалось загрузить историю'));
        break;
    }
  }

  Future<List<FileInformation>> _uploadFiles(List<PlatformFile>? files) async {
    if(files == null) {
      return [];
    }
    var result = await _uploadFilesUseCase.execute(files);
    switch (result) {
      case Ok():
        return result.value;
      default:
        return [];
    }
  }

  void _handleIncomingMessage(
      SupportChatMessage message,
      Emitter<SupportChatState> emit) {
    if (state is! SupportChatConnected) {
      emit(SupportChatError('Не удалось отправить сообщение'));
      return;
    }
    final connectedState = state as SupportChatConnected;
    final converted = _convertMessage(message);
    emit(connectedState.copyWith(messages: [...connectedState.messages, converted]));
  }

  SupportChatMessageBase _convertMessage(SupportChatMessage message) {
    return message.message;
  }

  @override
  Future<void> close() {
    _streamSubscription?.cancel();
    return super.close();
  }
}