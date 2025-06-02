import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatFileDocument extends StatelessWidget {
  final PlatformFile file;

  const SupportChatFileDocument({super.key, required this.file});

  @override
  Widget build(BuildContext context) {
    return Container(
      color: AppColors.basicGrey[700],
      child: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.insert_drive_file, size: 48, color: AppColors.textWhite),
            const SizedBox(height: 8),
            Text(
              file.name,
              style: const TextStyle(color: AppColors.textWhite),
              textAlign: TextAlign.center,
              maxLines: 2,
              overflow: TextOverflow.ellipsis,
            ),
          ],
        ),
      ),
    );
  }
}