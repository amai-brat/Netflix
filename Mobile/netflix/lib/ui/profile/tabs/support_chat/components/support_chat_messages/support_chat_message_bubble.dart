import 'package:flutter/material.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_messages/support_chat_message_file.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatMessageBubble extends StatelessWidget {
  final SupportChatMessageBase message;

  const SupportChatMessageBubble({super.key, required this.message});

  @override
  Widget build(BuildContext context) {
    final isUserMessage = message.role == 'user';

    return Align(
      alignment: isUserMessage ? Alignment.centerRight : Alignment.centerLeft,
      child: Container(
        margin: const EdgeInsets.symmetric(vertical: 8),
        constraints: BoxConstraints(
          maxWidth: MediaQuery.of(context).size.width * 0.8,
        ),
        child: Column(
          crossAxisAlignment: isUserMessage
              ? CrossAxisAlignment.end
              : CrossAxisAlignment.start,
          children: [
            Container(
              padding: const EdgeInsets.symmetric(
                vertical: 12,
                horizontal: 16,
              ),
              decoration: BoxDecoration(
                color: isUserMessage
                    ? AppColors.basicGrey[800]
                    : AppColors.basicGrey[700],
                borderRadius: BorderRadius.all(const Radius.circular(16)),
              ),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                spacing: 4,
                children: [
                  if (message.text.isNotEmpty)
                    Text(
                      isUserMessage ? 'Вы' : 'Поддержка',
                      style: TextStyle(
                        color: AppColors.basicGrey[400],
                        fontSize: 14,
                      ),
                    ),
                  if (message.files.isNotEmpty)
                    ...message.files.map(
                          (file) => SupportChatMessageFile(fileInformation: file),
                    ),
                  if (message.text.isNotEmpty)
                    Text(
                      message.text.trim()
                          .replaceAll(RegExp(r'\n+$'), '')
                          .replaceAll(RegExp(r'\n+'), '\n'),
                      style: const TextStyle(
                        color: AppColors.textWhite,
                        fontSize: 18,
                      ),
                    ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}