import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class SupportChatState {}

class SupportChatInitial extends SupportChatState {}

class SupportChatLoading extends SupportChatState {}

class SupportChatConnected extends SupportChatState {
  List<SupportChatMessageBase> messages;

  SupportChatConnected({
    this.messages = const [],
  });

  SupportChatConnected copyWith({
    List<SupportChatMessageBase>? messages,
  }) {
    return SupportChatConnected(
      messages: messages ?? this.messages,
    );
  }
}

class SupportChatError extends SupportChatState {
  final String message;

  SupportChatError(this.message);
}