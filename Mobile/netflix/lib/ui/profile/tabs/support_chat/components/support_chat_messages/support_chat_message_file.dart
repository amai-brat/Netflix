import 'dart:io';

import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatMessageFile extends StatelessWidget {
  final FileInformation fileInformation;

  const SupportChatMessageFile({super.key, required this.fileInformation});

  @override
  Widget build(BuildContext context) {
    final uri = Uri.parse(fileInformation.src);
    final file = File(uri.toFilePath());
    final platformFile = PlatformFile(
        name: fileInformation.name,
        path: file.path,
        size: file.lengthSync()
    );


    return Container(
        margin: const EdgeInsets.only(top: 8),
        padding: const EdgeInsets.all(8),
        decoration: BoxDecoration(
          color: AppColors.backgroundBlack,
          borderRadius: BorderRadius.circular(8),
        ),
        child: Row(
          children: [
            SupportChatFile(file: platformFile)
          ],
        ),
      );
  }
}