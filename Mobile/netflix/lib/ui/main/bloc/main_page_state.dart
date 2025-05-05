import 'package:netflix/domain/models/sections/section.dart';

enum MainPageStatus { initial, loading, completed }

class MainPageState {
  final MainPageStatus status;
  final List<Section> sections;
  final String error;

  MainPageState({
    required this.status,
    required this.sections,
    required this.error,
  });

  MainPageState.initial()
    : status = MainPageStatus.initial,
      sections = [],
      error = '';

  MainPageState copyWith({
    MainPageStatus? status,
    List<Section>? sections,
    String? error,
  }) {
    return MainPageState(
      status: status ?? this.status,
      sections: sections ?? this.sections,
      error: error ?? this.error,
    );
  }
}
