import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_file_upload_button.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_send_message_button.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_text_input.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatControl extends StatelessWidget {
  const SupportChatControl({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatBloc, SupportChatState>(
      builder: (context, state) {
        return Container(
          padding: const EdgeInsets.symmetric(
            vertical: 12,
            horizontal: 16,
          ),
          decoration: BoxDecoration(
            color: AppColors.inputGrey,
            border: Border(
              top: BorderSide(
                color: AppColors.inputGrey,
                width: 1,
              ),
            ),
          ),
          child: Row(
            children: [
              SupportChatFileUploadButton(),
              const SizedBox(width: 12),
              Expanded(child: SupportChatTextInput()),
              const SizedBox(width: 12),
              SupportChatSendMessageButton(),
            ],
          ),
        );
      },
    );
  }
}