import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_state.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatTextInput extends StatefulWidget {
  const SupportChatTextInput({super.key});

  @override
  State<SupportChatTextInput> createState() => _SupportChatTextInputState();
}

class _SupportChatTextInputState extends State<SupportChatTextInput> {
  final _controller = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatControlBloc, SupportChatControlState>(
        builder: (context, state) {
          final ctxMain = context.read<SupportChatBloc>();
          final ctx = context.read<SupportChatControlBloc>();
          if(state is SupportChatControlConnected){
            _controller.text = state.messageText;
          }

          return TextField(
            controller: _controller,
            style: const TextStyle(color: AppColors.textWhite, height: 2),
            decoration: InputDecoration(
              hintText: 'Введите сообщение...',
              hintStyle: TextStyle(color: AppColors.basicGrey),
              filled: true,
              fillColor: AppColors.inputGrey,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(12),
                borderSide: BorderSide.none,
              ),
              contentPadding: const EdgeInsets.symmetric(
                vertical: 12,
                horizontal: 16,
              ),
            ),
            minLines: 1,
            maxLines: 3,
            onChanged: (text) {
              ctx.add(MessageTextChangedEvent(text: text));
            },
            onSubmitted: (text) {
              if (state is SupportChatControlConnected) {
                ctxMain.add(SendMessageEvent(state.messageText, state.pickedFiles));
              }
            },
          );
        }
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }
}