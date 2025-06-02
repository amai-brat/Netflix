//
//  Generated code. Do not modify.
//  source: SupportChat.proto
//
// @dart = 3.3

// ignore_for_file: annotate_overrides, camel_case_types, comment_references
// ignore_for_file: constant_identifier_names, library_prefixes
// ignore_for_file: non_constant_identifier_names, prefer_final_fields
// ignore_for_file: unnecessary_import, unnecessary_this, unused_import

import 'dart:async' as $async;
import 'dart:core' as $core;

import 'package:grpc/service_api.dart' as $grpc;
import 'package:protobuf/protobuf.dart' as $pb;

import 'SupportChat.pb.dart' as $0;

export 'SupportChat.pb.dart';

@$pb.GrpcServiceName('support_chat.SupportChat')
class SupportChatClient extends $grpc.Client {
  /// The hostname for this service.
  static const $core.String defaultHost = '';

  /// OAuth scopes needed for the client.
  static const $core.List<$core.String> oauthScopes = [
    '',
  ];

  static final _$connect = $grpc.ClientMethod<$0.ConnectRequest, $0.ConnectResponse>(
      '/support_chat.SupportChat/Connect',
      ($0.ConnectRequest value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.ConnectResponse.fromBuffer(value));
  static final _$connectToStream = $grpc.ClientMethod<$0.ConnectToStreamRequest, $0.SupportChatMessage>(
      '/support_chat.SupportChat/ConnectToStream',
      ($0.ConnectToStreamRequest value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.SupportChatMessage.fromBuffer(value));
  static final _$sendMessage = $grpc.ClientMethod<$0.SupportChatSendMessage, $0.SupportChatMessageAck>(
      '/support_chat.SupportChat/SendMessage',
      ($0.SupportChatSendMessage value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.SupportChatMessageAck.fromBuffer(value));
  static final _$joinSupportChat = $grpc.ClientMethod<$0.JoinSupportChatRequest, $0.JoinSupportChatResponse>(
      '/support_chat.SupportChat/JoinSupportChat',
      ($0.JoinSupportChatRequest value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.JoinSupportChatResponse.fromBuffer(value));
  static final _$leaveSupportChat = $grpc.ClientMethod<$0.LeaveSupportChatRequest, $0.LeaveSupportChatResponse>(
      '/support_chat.SupportChat/LeaveSupportChat',
      ($0.LeaveSupportChatRequest value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.LeaveSupportChatResponse.fromBuffer(value));
  static final _$disconnect = $grpc.ClientMethod<$0.DisconnectRequest, $0.DisconnectResponse>(
      '/support_chat.SupportChat/Disconnect',
      ($0.DisconnectRequest value) => value.writeToBuffer(),
      ($core.List<$core.int> value) => $0.DisconnectResponse.fromBuffer(value));

  SupportChatClient(super.channel, {super.options, super.interceptors});

  $grpc.ResponseFuture<$0.ConnectResponse> connect($0.ConnectRequest request, {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$connect, request, options: options);
  }

  $grpc.ResponseStream<$0.SupportChatMessage> connectToStream($0.ConnectToStreamRequest request, {$grpc.CallOptions? options}) {
    return $createStreamingCall(_$connectToStream, $async.Stream.fromIterable([request]), options: options);
  }

  $grpc.ResponseFuture<$0.SupportChatMessageAck> sendMessage($0.SupportChatSendMessage request, {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$sendMessage, request, options: options);
  }

  $grpc.ResponseFuture<$0.JoinSupportChatResponse> joinSupportChat($0.JoinSupportChatRequest request, {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$joinSupportChat, request, options: options);
  }

  $grpc.ResponseFuture<$0.LeaveSupportChatResponse> leaveSupportChat($0.LeaveSupportChatRequest request, {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$leaveSupportChat, request, options: options);
  }

  $grpc.ResponseFuture<$0.DisconnectResponse> disconnect($0.DisconnectRequest request, {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$disconnect, request, options: options);
  }
}

@$pb.GrpcServiceName('support_chat.SupportChat')
abstract class SupportChatServiceBase extends $grpc.Service {
  $core.String get $name => 'support_chat.SupportChat';

  SupportChatServiceBase() {
    $addMethod($grpc.ServiceMethod<$0.ConnectRequest, $0.ConnectResponse>(
        'Connect',
        connect_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.ConnectRequest.fromBuffer(value),
        ($0.ConnectResponse value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.ConnectToStreamRequest, $0.SupportChatMessage>(
        'ConnectToStream',
        connectToStream_Pre,
        false,
        true,
        ($core.List<$core.int> value) => $0.ConnectToStreamRequest.fromBuffer(value),
        ($0.SupportChatMessage value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.SupportChatSendMessage, $0.SupportChatMessageAck>(
        'SendMessage',
        sendMessage_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.SupportChatSendMessage.fromBuffer(value),
        ($0.SupportChatMessageAck value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.JoinSupportChatRequest, $0.JoinSupportChatResponse>(
        'JoinSupportChat',
        joinSupportChat_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.JoinSupportChatRequest.fromBuffer(value),
        ($0.JoinSupportChatResponse value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.LeaveSupportChatRequest, $0.LeaveSupportChatResponse>(
        'LeaveSupportChat',
        leaveSupportChat_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.LeaveSupportChatRequest.fromBuffer(value),
        ($0.LeaveSupportChatResponse value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.DisconnectRequest, $0.DisconnectResponse>(
        'Disconnect',
        disconnect_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.DisconnectRequest.fromBuffer(value),
        ($0.DisconnectResponse value) => value.writeToBuffer()));
  }

  $async.Future<$0.ConnectResponse> connect_Pre($grpc.ServiceCall $call, $async.Future<$0.ConnectRequest> $request) async {
    return connect($call, await $request);
  }

  $async.Stream<$0.SupportChatMessage> connectToStream_Pre($grpc.ServiceCall $call, $async.Future<$0.ConnectToStreamRequest> $request) async* {
    yield* connectToStream($call, await $request);
  }

  $async.Future<$0.SupportChatMessageAck> sendMessage_Pre($grpc.ServiceCall $call, $async.Future<$0.SupportChatSendMessage> $request) async {
    return sendMessage($call, await $request);
  }

  $async.Future<$0.JoinSupportChatResponse> joinSupportChat_Pre($grpc.ServiceCall $call, $async.Future<$0.JoinSupportChatRequest> $request) async {
    return joinSupportChat($call, await $request);
  }

  $async.Future<$0.LeaveSupportChatResponse> leaveSupportChat_Pre($grpc.ServiceCall $call, $async.Future<$0.LeaveSupportChatRequest> $request) async {
    return leaveSupportChat($call, await $request);
  }

  $async.Future<$0.DisconnectResponse> disconnect_Pre($grpc.ServiceCall $call, $async.Future<$0.DisconnectRequest> $request) async {
    return disconnect($call, await $request);
  }

  $async.Future<$0.ConnectResponse> connect($grpc.ServiceCall call, $0.ConnectRequest request);
  $async.Stream<$0.SupportChatMessage> connectToStream($grpc.ServiceCall call, $0.ConnectToStreamRequest request);
  $async.Future<$0.SupportChatMessageAck> sendMessage($grpc.ServiceCall call, $0.SupportChatSendMessage request);
  $async.Future<$0.JoinSupportChatResponse> joinSupportChat($grpc.ServiceCall call, $0.JoinSupportChatRequest request);
  $async.Future<$0.LeaveSupportChatResponse> leaveSupportChat($grpc.ServiceCall call, $0.LeaveSupportChatRequest request);
  $async.Future<$0.DisconnectResponse> disconnect($grpc.ServiceCall call, $0.DisconnectRequest request);
}
