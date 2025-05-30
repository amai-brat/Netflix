import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
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
    return BlocBuilder<SupportChatBloc, SupportChatState>(
        builder: (context, state) {
          final ctx = context.read<SupportChatBloc>();
          if(state is SupportChatConnected){
            _controller.text = state.messageText;
          }

          return TextField(
            controller: _controller,
            style: const TextStyle(color: AppColors.textWhite),
            decoration: InputDecoration(
              hintText: 'Введите сообщение...',
              hintStyle: TextStyle(color: AppColors.inputGrey),
              filled: true,
              fillColor: AppColors.inputGrey,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(24),
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
              if (state is SupportChatConnected) {
                ctx.add(SendMessageEvent());
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