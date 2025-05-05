import 'dart:io';

import 'package:flutter/material.dart';
import 'package:netflix/domain/models/content/trailer_info.dart';
import 'package:netflix/ui/content/widgets/content_section.dart';
import 'package:youtube_player_iframe/youtube_player_iframe.dart';

class ContentTrailer extends StatefulWidget {
  final TrailerInfo trailerInfo;

  const ContentTrailer({super.key, required this.trailerInfo});

  @override
  State<ContentTrailer> createState() => _ContentTrailerState();
}

class _ContentTrailerState extends State<ContentTrailer> {
  late YoutubePlayerController _controller;

  @override
  void initState() {
    super.initState();

    if (!Platform.isLinux) {
      _controller = YoutubePlayerController.fromVideoId(
        videoId:
            YoutubePlayerController.convertUrlToId(widget.trailerInfo.url)!,
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return ContentSection(
      title: 'Трейлер',
      children: [
        !Platform.isLinux
            ? YoutubePlayer(controller: _controller)
            : Text('Типа плеер'),
        Text(
          widget.trailerInfo.name,
          style: Theme.of(context).textTheme.bodyLarge,
        ),
      ],
    );
  }
}
