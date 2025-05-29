import 'package:flutter/material.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_messages/support_chat_message_bubble.dart';

class SupportChatMessageList extends StatefulWidget {
  final List<SupportChatMessageBase> messages;

  const SupportChatMessageList({super.key, required this.messages});

  @override
  State<SupportChatMessageList> createState() => _SupportChatMessagesListState();
}

class _SupportChatMessagesListState extends State<SupportChatMessageList> {
  final _scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) => _scrollToBottom());
  }

  @override
  void didUpdateWidget(SupportChatMessageList oldWidget) {
    super.didUpdateWidget(oldWidget);
    if (widget.messages.length > oldWidget.messages.length) {
      _scrollToBottom();
    }
  }

  void _scrollToBottom() {
    if (_scrollController.hasClients) {
      _scrollController.animateTo(
        _scrollController.position.maxScrollExtent,
        duration: const Duration(milliseconds: 200),
        curve: Curves.easeOut,
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      controller: _scrollController,
      padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 12),
      itemCount: widget.messages.length,
      itemBuilder: (context, index) {
        return SupportChatMessageBubble(message: widget.messages[index]);
      },
    );
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }
}