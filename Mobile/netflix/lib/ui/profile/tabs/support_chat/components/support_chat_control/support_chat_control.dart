import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_file_upload_button.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_send_message_button.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_text_input.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatControl extends StatelessWidget {
  const SupportChatControl({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatControlBloc, SupportChatControlState>(
      builder: (context, state) {
        return Container(
          padding: EdgeInsets.symmetric(
            vertical: 12,
            horizontal: 8,
          ),
          decoration: BoxDecoration(
            color: AppColors.basicGrey[800],
            border: Border(
              top: BorderSide(
                color: AppColors.basicGrey[700]!,
                width: 2,
              ),
            ),
          ),
          child: Row(
            spacing: 12,
            children: [
              SupportChatFileUploadButton(),
              Expanded(child: SupportChatTextInput()),
              SupportChatSendMessageButton(),
            ],
          ),
        );
      },
    );
  }
}