import 'dart:io';
import 'package:audioplayers/audioplayers.dart';
import 'package:file_picker/file_picker.dart';
import 'package:flutter/material.dart';
import 'package:mime_type/mime_type.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:video_player/video_player.dart';

class SupportChatFileAudio extends StatefulWidget {
  final PlatformFile file;

  const SupportChatFileAudio({super.key, required this.file});

  @override
  State<SupportChatFileAudio> createState() => _SupportChatFileAudioState();
}

class _SupportChatFileAudioState extends State<SupportChatFileAudio> {
  AudioPlayer? _audioPlayer;
  PlayerState _audioPlayerState = PlayerState.stopped;
  Duration _audioDuration = Duration.zero;
  Duration _audioPosition = Duration.zero;

  @override
  void initState() {
    super.initState();
    _initAudio();
  }

  Future<void> _initAudio() async {
    if (widget.file.path != null && widget.file.path!.isNotEmpty) {
      final mimeType = mime(widget.file.name) ?? 'application/octet-stream';

      if (mimeType.contains('audio')) {
        _audioPlayer = AudioPlayer();
        _audioPlayer!.onPlayerStateChanged.listen((state) {
          setState(() => _audioPlayerState = state);
        });
        _audioPlayer!.onDurationChanged.listen((duration) {
          setState(() => _audioDuration = duration);
        });
        _audioPlayer!.onPositionChanged.listen((position) {
          setState(() => _audioPosition = position);
        });
      }
    }
  }

  Future<void> _playAudio() async {
    if (_audioPlayerState == PlayerState.playing) {
      await _audioPlayer!.pause();
    } else {
      await _audioPlayer!.play(DeviceFileSource(widget.file.path!));
    }
  }

  Future<void> _stopAudio() async {
    await _audioPlayer!.stop();
    setState(() => _audioPosition = Duration.zero);
  }

  @override
  void dispose() {
    _audioPlayer?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    if (widget.file.path != null && widget.file.path!.isNotEmpty) {
      final mimeType = mime(widget.file.name) ?? 'application/octet-stream';

      if (mimeType.contains('audio')) {
        return Padding(
          padding: EdgeInsets.symmetric(
            vertical: 30,
            horizontal: 2
          ),
          child: Column(
            spacing: 8,
            children: [
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                spacing: 4,
                children: [
                  Text(
                    widget.file.name,
                    style: const TextStyle(color: AppColors.textWhite),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  ),
                  LinearProgressIndicator(
                    value: _audioDuration.inMilliseconds > 0
                        ? _audioPosition.inMilliseconds / _audioDuration.inMilliseconds
                        : 0,
                    backgroundColor: AppColors.basicGrey[600],
                    color: AppColors.textWhite,
                  ),
                ],
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    _formatDuration(_audioPosition),
                    style: TextStyle(color: AppColors.basicGrey[400], fontSize: 12),
                  ),
                  Text(
                    _formatDuration(_audioDuration),
                    style: TextStyle(color: AppColors.basicGrey[400], fontSize: 12),
                  ),
                ],
              ),
              Row(
                children: [
                  IconButton(
                      icon: Icon(
                        _audioPlayerState == PlayerState.playing
                            ? Icons.pause
                            : Icons.play_arrow,
                        color: AppColors.textWhite,
                        size: 20,
                      ),
                      onPressed: _playAudio
                  ),
                  Expanded(child: const SizedBox()),
                  IconButton(
                    icon: const Icon(Icons.stop, color: AppColors.textWhite, size: 20),
                    onPressed: _stopAudio,
                  ),
                ],
              ),
            ],
          ),
        );
      }
    }

    return const Center(child: Text('Не аудио'));
  }

  String _formatDuration(Duration duration) {
    return '${duration.inMinutes}:${(duration.inSeconds % 60).toString().padLeft(2, '0')}';
  }
}