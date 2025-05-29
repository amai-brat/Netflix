import 'package:netflix/grpc_generated/SupportChat.pb.dart';

class FileInformationDto {
  final String src;
  final String type;
  final String name;

  FileInformationDto({
    required this.src,
    required this.type,
    required this.name
  });

  FileInformationDto.fromMap(Map<String, dynamic> map)
      : src = map['src'],
        type = map['type'],
        name = map['name'];


  FileInformation toFileInformation() => FileInformation(
      src: src,
      type: type,
      name: name
  );
}
