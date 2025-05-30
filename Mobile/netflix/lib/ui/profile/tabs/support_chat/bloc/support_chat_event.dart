import 'package:file_picker/file_picker.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class SupportChatEvent {}

class ConnectSupportChatEvent extends SupportChatEvent {}

class SendMessageEvent extends SupportChatEvent {
  final String messageText;
  final List<PlatformFile>? pickedFiles;

  SendMessageEvent(this.messageText, this.pickedFiles);
}

class IncomingMessageEvent extends SupportChatEvent {
  final SupportChatMessage message;

  IncomingMessageEvent(this.message);
}

class DisconnectSupportChatEvent extends SupportChatEvent {}