import 'package:file_picker/file_picker.dart';
import 'package:netflix/data/models/file_information_dto.dart';
import 'package:netflix/data/services/file_service.dart';
import 'package:netflix/domain/repositories/file_repository.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

class FileRepositoryImpl extends FileRepository {
  final FileService service;

  FileRepositoryImpl({required this.service});

  @override
  Future<List<FileInformation>> uploadFiles(List<PlatformFile> files) async {
    final response = await service.uploadFiles(files);
    return response
        .asMap()
        .map((i, url) =>
        MapEntry(i, FileInformationDto(src: url, type: files[i].extension!, name: files[i].name)
            .toFileInformation()))
        .values
        .toList();
  }
}