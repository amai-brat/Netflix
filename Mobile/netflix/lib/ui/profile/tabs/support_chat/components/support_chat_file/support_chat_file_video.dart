import 'dart:io';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:mime_type/mime_type.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:video_player/video_player.dart';

class SupportChatFileVideo extends StatefulWidget {
  final PlatformFile file;

  const SupportChatFileVideo({super.key, required this.file});

  @override
  State<SupportChatFileVideo> createState() => _SupportChatFileVideoState();
}

class _SupportChatFileVideoState extends State<SupportChatFileVideo> {
  VideoPlayerController? _videoController;
  bool _isVideoInitialized = false;

  @override
  void initState() {
    super.initState();
    _initVideo();
  }

  Future<void> _initVideo() async {
    if (widget.file.path != null && widget.file.path!.isNotEmpty) {
      final mimeType = mime(widget.file.name) ?? 'application/octet-stream';
      if (mimeType.contains('video')) {
        _videoController = VideoPlayerController.file(File(widget.file.path!));
        await _videoController?.initialize();
        setState(() => _isVideoInitialized = true);
      }
    }
  }

  @override
  void dispose() {
    _videoController?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (widget.file.path != null && widget.file.path!.isNotEmpty) {
      final mimeType = mime(widget.file.name) ?? 'application/octet-stream';

      if (mimeType.contains('video')) {
        if (!_isVideoInitialized || _videoController == null) {
          return const Center(child: CircularProgressIndicator());
        }

        return Stack(
          alignment: Alignment.center,
          children: [
            AspectRatio(
              aspectRatio: _videoController!.value.aspectRatio,
              child: VideoPlayer(_videoController!),
            ),
            IconButton(
              icon: Icon(
                _videoController!.value.isPlaying ? Icons.pause : Icons.play_arrow,
                color: AppColors.textWhite,
                size: 48,
              ),
              onPressed: () {
                setState(() {
                  _videoController!.value.isPlaying
                      ? _videoController!.pause()
                      : _videoController!.play();
                });
              },
            ),
          ],
        );;
      }
    }

    return const Center(child: Text('Не видео'));
  }
}