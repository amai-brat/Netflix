import 'package:netflix/data/models/file_information_dto.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

class SupportChatHistoryMessage {
  final List<FileInformationDto> files;
  final String role;
  final String text;

  SupportChatHistoryMessage({
    required this.files,
    required this.role,
    required this.text
  });

  SupportChatHistoryMessage.fromMap(Map<String, dynamic> map)
      : files = (map['files'] as List<dynamic>?)?.map((fileMap) =>
                  FileInformationDto.fromMap(fileMap as Map<String, dynamic>)).toList() ?? [],
        role = map['role'],
        text = map['text'];

  SupportChatMessageBase toSupportChatMessageBase() => SupportChatMessageBase(
    files: files.map((f) => f.toFileInformation()).toList(),
    role: role,
    text: text
  );
}