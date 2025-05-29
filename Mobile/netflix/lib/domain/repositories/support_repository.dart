import 'package:netflix/grpc_generated/SupportChat.pb.dart';

abstract class SupportRepository {
  Future<List<SupportChatMessageBase>> getHistory();
}