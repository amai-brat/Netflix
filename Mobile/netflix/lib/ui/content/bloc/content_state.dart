import 'package:netflix/domain/models/content/content.dart';

class ContentState {
  final bool isLoading;
  final Content? content;
  final int contentViews;
  final String error;

  ContentState({
    required this.isLoading,
    required this.content,
    required this.contentViews,
    required this.error,
  });

  ContentState.initial()
    : content = null,
      error = '',
      isLoading = true,
      contentViews = 0;

  ContentState copyWith({
    bool? isLoading,
    Content? Function()? content,
    int? contentViews,
    String? error,
  }) {
    return ContentState(
      isLoading: isLoading ?? this.isLoading,
      content: content != null ? content() : this.content,
      contentViews: contentViews ?? this.contentViews,
      error: error ?? this.error,
    );
  }
}
