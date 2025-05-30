import 'package:file_picker/file_picker.dart';

abstract class SupportChatControlState {}

class SupportChatControlInitial extends SupportChatControlState {}

class SupportChatControlConnected extends SupportChatControlState {
  final String messageText;
  final List<PlatformFile>? pickedFiles;

  SupportChatControlConnected({
    this.messageText = '',
    this.pickedFiles,
  });

  SupportChatControlConnected copyWith({
    String? messageText,
    (List<PlatformFile>?, bool isNull)? pickedFiles,
  }) {
    return SupportChatControlConnected(
        messageText: messageText ?? this.messageText,
        pickedFiles: pickedFiles == null ? this.pickedFiles : pickedFiles.$2 ? null : pickedFiles.$1 ?? this.pickedFiles
    );
  }
}