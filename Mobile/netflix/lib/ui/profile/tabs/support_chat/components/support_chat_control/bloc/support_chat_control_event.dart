import 'package:file_picker/file_picker.dart';

abstract class SupportChatControlEvent {}

class MessageTextChangedEvent extends SupportChatControlEvent {
  final String text;
  MessageTextChangedEvent({required this.text});
}

class FilesSelectedEvent extends SupportChatControlEvent {
  final List<PlatformFile> files;
  FilesSelectedEvent({required this.files});
}

class FileRemovedEvent extends SupportChatControlEvent {
  final PlatformFile file;
  FileRemovedEvent({required this.file});
}

class ResetEvent extends SupportChatControlEvent {}