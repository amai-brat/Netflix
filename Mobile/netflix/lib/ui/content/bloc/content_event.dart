abstract class ContentEvent {}

class ContentPageOpened extends ContentEvent {
  final int contentId;

  ContentPageOpened({required this.contentId});
}