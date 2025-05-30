import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:netflix/clients/file_client.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:netflix/utils/di.dart';

class SupportChatMessageFile extends StatelessWidget {
  final FileInformation fileInformation;

  const SupportChatMessageFile({super.key, required this.fileInformation});

  @override
  Widget build(BuildContext context) {
    final fileClient = locator<FileClient>();

    return FutureBuilder<PlatformFile?>(
      future: fileClient.downloadFile(fileInformation.src),
      builder: (BuildContext context, AsyncSnapshot<PlatformFile?> snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return CircularProgressIndicator();
        } else if (snapshot.hasError) {
          return Text('Ошибка: ${snapshot.error}');
        } else if (!snapshot.hasData) {
          return Text('Файл не найден');
        } else {
          final platformFile = snapshot.data!;
          return Container(
            margin: const EdgeInsets.only(top: 8),
            padding: const EdgeInsets.all(8),
            decoration: BoxDecoration(
              color: AppColors.backgroundBlack,
              borderRadius: BorderRadius.circular(8),
            ),
            child: Center(
              child: SupportChatFile(file: platformFile),
            )
          );
        }
      },
    );
  }
}