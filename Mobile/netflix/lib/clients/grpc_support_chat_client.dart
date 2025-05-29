import 'package:fixnum/fixnum.dart';
import 'package:grpc/grpc.dart';
import 'package:netflix/grpc_generated/SupportChat.pbgrpc.dart';

class GrpcSupportChatClient {
  final SupportChatClient _client;

  GrpcSupportChatClient(String host, int port) :
        _client = SupportChatClient(ClientChannel(host, port: port, options: const ChannelOptions(credentials: ChannelCredentials.secure())));

  Future<ConnectResponse> connect(Map<String, String> metadata) async {
    return _client.connect(
      ConnectRequest(),
      options: CallOptions(metadata: metadata),
    );
  }

  Stream<SupportChatMessage> connectToStream(
      String sessionId,
      Map<String, String> metadata,
      ) {
    final request = ConnectToStreamRequest()..sessionId = sessionId;
    return _client.connectToStream(
      request,
      options: CallOptions(metadata: metadata),
    );
  }

  Future<void> disconnect(String sessionId, Map<String, String> metadata) async {
    final request = DisconnectRequest()..sessionId = sessionId;
    await _client.disconnect(
      request,
      options: CallOptions(metadata: metadata),
    );
  }

  Future<SupportChatMessageAck> sendMessage(
      Int64 groupId,
      String sessionId,
      String message,
      List<FileInformation> files,
      Map<String, String> metadata,
      ) async {
    final request = SupportChatSendMessage()
      ..groupOwnerId = groupId
      ..sessionId = sessionId
      ..message = message
      ..files.addAll(files);

    return _client.sendMessage(
      request,
      options: CallOptions(metadata: metadata),
    );
  }
}