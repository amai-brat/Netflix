import 'package:netflix/domain/models/content/content.dart';

class ContentState {
  final bool isLoading;
  final Content? content;
  final String error;

  ContentState({
    required this.isLoading,
    required this.content,
    required this.error,
  });

  ContentState.initial() : content = null, error = '', isLoading = true;

  ContentState copyWith({
    bool? isLoading,
    Content? Function()? content,
    String? error,
  }) {
    return ContentState(
      isLoading: isLoading ?? this.isLoading,
      content: content != null ? content() : this.content,
      error: error ?? this.error,
    );
  }
}
