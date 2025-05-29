import 'package:flutter/material.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/utils/app_colors.dart';

class SupportChatMessageFile extends StatelessWidget {
  final FileInformation fileInformation;

  const SupportChatMessageFile({super.key, required this.fileInformation});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () => {},
      child: Container(
        margin: const EdgeInsets.only(top: 8),
        padding: const EdgeInsets.all(8),
        decoration: BoxDecoration(
          color: AppColors.backgroundBlack,
          borderRadius: BorderRadius.circular(8),
        ),
        child: Row(
          children: [
            Icon(
              _getFileIcon(fileInformation.type),
              color: AppColors.textWhite,
            ),
            const SizedBox(width: 8),
            Expanded(
              child: Text(
                fileInformation.name,
                style: const TextStyle(
                  color: AppColors.textWhite,
                  overflow: TextOverflow.ellipsis,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  IconData _getFileIcon(String type) {
    if (type.contains('image')) return Icons.image;
    if (type.contains('video')) return Icons.videocam;
    if (type.contains('audio')) return Icons.audiotrack;
    return Icons.insert_drive_file;
  }
}