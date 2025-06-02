import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:mime_type/mime_type.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file_audio.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file_document.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file_image.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file_video.dart';

class SupportChatFile extends StatelessWidget {
  final PlatformFile file;

  const SupportChatFile({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    if (file.path != null && file.path!.isNotEmpty) {
      final mimeType = mime(file.name) ?? 'application/octet-stream';

      if (mimeType.contains('image')) {
        return SupportChatFileImage(file: file);
      } else if (mimeType.contains('video')) {
        return SupportChatFileVideo(file: file);
      } else if (mimeType.contains('audio')) {
        return SupportChatFileAudio(file: file);
      }
    }

    return SupportChatFileDocument(file: file);
  }
}