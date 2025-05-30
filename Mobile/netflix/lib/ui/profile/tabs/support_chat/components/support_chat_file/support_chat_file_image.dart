import 'dart:io';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:mime_type/mime_type.dart';

class SupportChatFileImage extends StatelessWidget {
  final PlatformFile file;

  const SupportChatFileImage({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    if (file.path != null && file.path!.isNotEmpty) {
      final mimeType = mime(file.name) ?? 'application/octet-stream';

      if (mimeType.contains('image')) {
        return Image.file(
          File(file.path!),
          fit: BoxFit.cover,
        );
      }
    }

    return const Center(child: Text('Не картинка'));
  }
}