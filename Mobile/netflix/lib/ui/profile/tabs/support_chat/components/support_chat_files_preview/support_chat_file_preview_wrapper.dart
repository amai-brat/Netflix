import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_state.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_files_preview/support_chat_file_preview.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatFilePreviewWrapper extends StatelessWidget {
  final PlatformFile file;

  const SupportChatFilePreviewWrapper({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatBloc, SupportChatState>(
        builder: (context, state){
          final ctx = context.read<SupportChatBloc>();
          return Stack(
            children: [
              Container(
                width: 120,
                height: 120,
                decoration: BoxDecoration(
                  color: AppColors.inputGrey,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: SupportChatFilePreview(file: file),
              ),
              Positioned(
                top: -5,
                right: -5,
                child: IconButton(
                  icon: Container(
                    decoration: BoxDecoration(
                      shape: BoxShape.circle,
                      color: AppColors.inputGrey,
                    ),
                    child: const Icon(
                      Icons.close,
                      size: 16,
                      color: AppColors.textWhite,
                    ),
                  ),
                  onPressed: () => ctx.add(FileRemovedEvent(file: file)),
                ),
              ),
            ],
          );
        });
  }
}