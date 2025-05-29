import 'package:file_picker/file_picker.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class SupportChatState {}

class SupportChatInitial extends SupportChatState {}

class SupportChatLoading extends SupportChatState {}

class SupportChatConnected extends SupportChatState {
  final String messageText;
  final List<PlatformFile>? pickedFiles;
  final List<SupportChatMessageBase> messages;

  SupportChatConnected({
    this.messageText = '',
    this.pickedFiles,
    this.messages = const [],
  });

  SupportChatConnected copyWith({
    String? messageText,
    (List<PlatformFile>?, bool isNull)? pickedFiles,
    List<SupportChatMessageBase>? messages
  }) {
    return SupportChatConnected(
      messageText: messageText ?? this.messageText,
      pickedFiles: pickedFiles == null ? this.pickedFiles : pickedFiles.$2 ? null : pickedFiles.$1 ?? this.pickedFiles,
      messages: messages ?? this.messages
    );
  }
}

class SupportChatError extends SupportChatState {
  final String message;

  SupportChatError(this.message);
}