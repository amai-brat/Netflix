import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_file/support_chat_file.dart';

class SupportChatFilePreview extends StatelessWidget {
  final PlatformFile file;

  const SupportChatFilePreview({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    return SupportChatFile(file: file);
  }
}