import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_files_preview/support_chat_files_preview.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_messages/support_chat_message_list.dart';

class SupportChatBody extends StatelessWidget{
  const SupportChatBody({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatBloc, SupportChatState>(
      builder: (context, state) {
        if (state is SupportChatLoading) {
          return const Center(child: CircularProgressIndicator());
        } else if (state is SupportChatError) {
          return Center(child: Text(state.message));
        } else if (state is SupportChatConnected) {
          return Column(
            children: [
              Expanded(child: SupportChatMessageList(messages: state.messages)),
              const SupportChatFilesPreview(),
            ],
          );
        }
        return const SizedBox.shrink();
      },
    );
  }
}