import 'package:netflix/domain/repositories/support_repository.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';
import 'package:netflix/utils/result.dart';

class GetHistoryUseCase {
  final SupportRepository _supportRepository;

  GetHistoryUseCase({
    required SupportRepository supportRepository
  }) : _supportRepository = supportRepository;

  Future<Result<List<SupportChatMessageBase>>> execute() async {
    try {
      return Result.ok(await _supportRepository.getHistory());
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
