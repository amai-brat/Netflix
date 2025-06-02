import 'package:file_picker/file_picker.dart';
import 'package:netflix/clients/file_client.dart';

class FileService {
  final FileClient _uploadClient;

  FileService(this._uploadClient);

  Future<List<String>> uploadFiles(List<PlatformFile> files) async {
    return _uploadClient.uploadFiles(files: files);
  }
}