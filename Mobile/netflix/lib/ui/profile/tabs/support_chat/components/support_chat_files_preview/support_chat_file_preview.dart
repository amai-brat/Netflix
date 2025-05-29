import 'dart:io';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatFilePreview extends StatelessWidget {
  static const _images = ['jpeg', 'png', 'gif', 'svg'];
  static const _videos = ['mp4', 'webm', 'avi', 'mkv'];
  static const _audios = ['mp3', 'wav', 'aac'];

  final PlatformFile file;

  const SupportChatFilePreview({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    if (file.path != null && file.path!.isNotEmpty) {
      final ext = (file.extension ?? '').toLowerCase();

      if (ext == 'jpg' || ext == 'jpeg' || ext == 'png') {
        return Image.file(
          File(file.path!),
          fit: BoxFit.cover,
        );
      }
    }

    return Center(
      child: Icon(
        _getFileIcon(file.extension),
        size: 32,
        color: AppColors.inputGrey,
      ),
    );
  }

  IconData _getFileIcon(String? type) {
    if (type == null) return Icons.insert_drive_file;
    if (_images.any((i) => type == i)) return Icons.image;
    if (_videos.any((v) => type == v)) return Icons.videocam;
    if (_audios.any((a) => type == a)) return Icons.audiotrack;
    return Icons.insert_drive_file;
  }
}