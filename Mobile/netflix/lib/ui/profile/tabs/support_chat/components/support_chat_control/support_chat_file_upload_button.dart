import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_state.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatFileUploadButton extends StatelessWidget {
  const SupportChatFileUploadButton({super.key});


  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SupportChatControlBloc, SupportChatControlState>(
      builder: (context, state) {
        final ctx = context.read<SupportChatControlBloc>();
        return IconButton(
          icon: const Icon(Icons.attach_file, size: 36),
          color: AppColors.primaryRed,
          onPressed: () => _selectFiles(ctx),
        );
      },
    );
  }

  Future<void> _selectFiles(SupportChatControlBloc ctx) async {
    final result = await FilePicker.platform.pickFiles(
      allowMultiple: true
    );

    if (result != null && result.files.isNotEmpty) {
      ctx.add(FilesSelectedEvent(files: result.files));
    }
  }
}