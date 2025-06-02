import 'package:file_picker/file_picker.dart';
import 'package:netflix/domain/repositories/file_repository.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/utils/result.dart';

class UploadFilesUseCase {
  final FileRepository _fileRepository;

  UploadFilesUseCase({
    required FileRepository fileRepository
  }) : _fileRepository = fileRepository;

  Future<Result<List<FileInformation>>> execute(List<PlatformFile> files) async {
    if(files.isEmpty){
      return Result.ok([]);
    }
    try {
      return Result.ok(await _fileRepository.uploadFiles(files));
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
