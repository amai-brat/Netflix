import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_state.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatSendMessageButton extends StatelessWidget {
  const SupportChatSendMessageButton({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatControlBloc, SupportChatControlState>(
      builder: (context, state) {
        final canSend = (state is SupportChatControlConnected) && state.messageText.isNotEmpty;

        return IconButton(
          icon: Icon(
            Icons.send,
            size: 36,
            color: canSend ? AppColors.primaryRed : AppColors.inputGrey,
          ),
          onPressed: canSend
              ? () => _sendMessages(context, state)
              : null,
        );
      },
    );
  }

  void _sendMessages(BuildContext context, SupportChatControlState state)
  {
    final conState = state as SupportChatControlConnected;
    final ctxMain = context.read<SupportChatBloc>();
    final ctx = context.read<SupportChatControlBloc>();
    ctxMain.add(SendMessageEvent(conState.messageText, conState.pickedFiles));
    ctx.add(ResetEvent());
  }
}