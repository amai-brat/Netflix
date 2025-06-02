import 'package:netflix/data/services/support_service.dart';
import 'package:netflix/domain/repositories/support_repository.dart';
import 'package:netflix/grpc_generated/SupportChat.pb.dart';

class SupportRepositoryImpl extends SupportRepository {
  final SupportService service;

  SupportRepositoryImpl({required this.service});

  @override
  Future<List<SupportChatMessageBase>> getHistory() async {
    final history = await service.getHistory();
    return history.map((m) => m.toSupportChatMessageBase()).toList();
  }
}