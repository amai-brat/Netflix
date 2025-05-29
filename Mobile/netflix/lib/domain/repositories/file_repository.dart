import 'package:file_picker/file_picker.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class FileRepository {
  Future<List<FileInformation>> uploadFiles(List<PlatformFile> files);
}