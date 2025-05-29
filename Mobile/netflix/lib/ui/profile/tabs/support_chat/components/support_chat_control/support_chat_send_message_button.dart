import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatSendMessageButton extends StatelessWidget {
  const SupportChatSendMessageButton({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatBloc, SupportChatState>(
      builder: (context, state) {
        final canSend = (state is SupportChatConnected) && state.messageText.isNotEmpty;
        final ctx = context.read<SupportChatBloc>();

        return IconButton(
          icon: Icon(
            Icons.send,
            size: 28,
            color: canSend ? AppColors.primaryRed : AppColors.inputGrey,
          ),
          onPressed: canSend
              ? () => ctx.add(SendMessageEvent())
              : null,
        );
      },
    );
  }
}