import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_files_preview/support_chat_file_preview_wrapper.dart';

class SupportChatFilesPreview extends StatelessWidget {
  const SupportChatFilesPreview({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatBloc, SupportChatState>(
      builder: (context, state) {
        if(state is! SupportChatConnected) {
          return const Center(child: CircularProgressIndicator());
        }
        final files = state.pickedFiles;
        if (files == null || files.isEmpty) return const SizedBox.shrink();

        return Container(
          height: 200,
          color: Colors.transparent,
          padding: const EdgeInsets.all(6),
          child: ListView.separated(
            scrollDirection: Axis.horizontal,
            itemCount: files.length,
            separatorBuilder: (_, __) => const SizedBox(width: 8),
            itemBuilder: (context, index) {
              return SupportChatFilePreviewWrapper(file: files[index]);
            },
          ),
        );
      },
    );
  }
}