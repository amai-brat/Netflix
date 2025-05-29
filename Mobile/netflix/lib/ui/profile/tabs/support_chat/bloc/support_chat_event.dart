import 'package:file_picker/file_picker.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class SupportChatEvent {}

class ConnectSupportChatEvent extends SupportChatEvent {}

class MessageTextChangedEvent extends SupportChatEvent {
  final String text;
  MessageTextChangedEvent({required this.text});
}

class FilesSelectedEvent extends SupportChatEvent {
  final List<PlatformFile> files;
  FilesSelectedEvent({required this.files});
}

class FileRemovedEvent extends SupportChatEvent {
  final PlatformFile file;
  FileRemovedEvent({required this.file});
}

class SendMessageEvent extends SupportChatEvent {}

class IncomingMessageEvent extends SupportChatEvent {
  final SupportChatMessage message;

  IncomingMessageEvent(this.message);
}

class DisconnectSupportChatEvent extends SupportChatEvent {}