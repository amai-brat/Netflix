//
//  Generated code. Do not modify.
//  source: SupportChat.proto
//
// @dart = 3.3

// ignore_for_file: annotate_overrides, camel_case_types, comment_references
// ignore_for_file: constant_identifier_names, library_prefixes
// ignore_for_file: non_constant_identifier_names, prefer_final_fields
// ignore_for_file: unnecessary_import, unnecessary_this, unused_import

import 'dart:core' as $core;

import 'package:fixnum/fixnum.dart' as $fixnum;
import 'package:protobuf/protobuf.dart' as $pb;

import 'SupportChat.pbenum.dart';

export 'package:protobuf/protobuf.dart' show GeneratedMessageGenericExtensions;

export 'SupportChat.pbenum.dart';

class ConnectRequest extends $pb.GeneratedMessage {
  factory ConnectRequest() => create();
  ConnectRequest._() : super();
  factory ConnectRequest.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory ConnectRequest.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'ConnectRequest', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectRequest clone() => ConnectRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectRequest copyWith(void Function(ConnectRequest) updates) => super.copyWith((message) => updates(message as ConnectRequest)) as ConnectRequest;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static ConnectRequest create() => ConnectRequest._();
  ConnectRequest createEmptyInstance() => create();
  static $pb.PbList<ConnectRequest> createRepeated() => $pb.PbList<ConnectRequest>();
  @$core.pragma('dart2js:noInline')
  static ConnectRequest getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<ConnectRequest>(create);
  static ConnectRequest? _defaultInstance;
}

class ConnectToStreamRequest extends $pb.GeneratedMessage {
  factory ConnectToStreamRequest({
    $core.String? sessionId,
  }) {
    final $result = create();
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    return $result;
  }
  ConnectToStreamRequest._() : super();
  factory ConnectToStreamRequest.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory ConnectToStreamRequest.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'ConnectToStreamRequest', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectToStreamRequest clone() => ConnectToStreamRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectToStreamRequest copyWith(void Function(ConnectToStreamRequest) updates) => super.copyWith((message) => updates(message as ConnectToStreamRequest)) as ConnectToStreamRequest;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static ConnectToStreamRequest create() => ConnectToStreamRequest._();
  ConnectToStreamRequest createEmptyInstance() => create();
  static $pb.PbList<ConnectToStreamRequest> createRepeated() => $pb.PbList<ConnectToStreamRequest>();
  @$core.pragma('dart2js:noInline')
  static ConnectToStreamRequest getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<ConnectToStreamRequest>(create);
  static ConnectToStreamRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get sessionId => $_getSZ(0);
  @$pb.TagNumber(1)
  set sessionId($core.String v) { $_setString(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasSessionId() => $_has(0);
  @$pb.TagNumber(1)
  void clearSessionId() => $_clearField(1);
}

class ConnectResponse extends $pb.GeneratedMessage {
  factory ConnectResponse({
    $core.String? sessionId,
  }) {
    final $result = create();
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    return $result;
  }
  ConnectResponse._() : super();
  factory ConnectResponse.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory ConnectResponse.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'ConnectResponse', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectResponse clone() => ConnectResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ConnectResponse copyWith(void Function(ConnectResponse) updates) => super.copyWith((message) => updates(message as ConnectResponse)) as ConnectResponse;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static ConnectResponse create() => ConnectResponse._();
  ConnectResponse createEmptyInstance() => create();
  static $pb.PbList<ConnectResponse> createRepeated() => $pb.PbList<ConnectResponse>();
  @$core.pragma('dart2js:noInline')
  static ConnectResponse getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<ConnectResponse>(create);
  static ConnectResponse? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get sessionId => $_getSZ(0);
  @$pb.TagNumber(1)
  set sessionId($core.String v) { $_setString(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasSessionId() => $_has(0);
  @$pb.TagNumber(1)
  void clearSessionId() => $_clearField(1);
}

class JoinSupportChatRequest extends $pb.GeneratedMessage {
  factory JoinSupportChatRequest({
    $fixnum.Int64? groupOwnerId,
    $core.String? sessionId,
  }) {
    final $result = create();
    if (groupOwnerId != null) {
      $result.groupOwnerId = groupOwnerId;
    }
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    return $result;
  }
  JoinSupportChatRequest._() : super();
  factory JoinSupportChatRequest.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory JoinSupportChatRequest.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'JoinSupportChatRequest', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aInt64(1, _omitFieldNames ? '' : 'groupOwnerId')
    ..aOS(2, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  JoinSupportChatRequest clone() => JoinSupportChatRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  JoinSupportChatRequest copyWith(void Function(JoinSupportChatRequest) updates) => super.copyWith((message) => updates(message as JoinSupportChatRequest)) as JoinSupportChatRequest;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static JoinSupportChatRequest create() => JoinSupportChatRequest._();
  JoinSupportChatRequest createEmptyInstance() => create();
  static $pb.PbList<JoinSupportChatRequest> createRepeated() => $pb.PbList<JoinSupportChatRequest>();
  @$core.pragma('dart2js:noInline')
  static JoinSupportChatRequest getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<JoinSupportChatRequest>(create);
  static JoinSupportChatRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $fixnum.Int64 get groupOwnerId => $_getI64(0);
  @$pb.TagNumber(1)
  set groupOwnerId($fixnum.Int64 v) { $_setInt64(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasGroupOwnerId() => $_has(0);
  @$pb.TagNumber(1)
  void clearGroupOwnerId() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get sessionId => $_getSZ(1);
  @$pb.TagNumber(2)
  set sessionId($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasSessionId() => $_has(1);
  @$pb.TagNumber(2)
  void clearSessionId() => $_clearField(2);
}

class JoinSupportChatResponse extends $pb.GeneratedMessage {
  factory JoinSupportChatResponse() => create();
  JoinSupportChatResponse._() : super();
  factory JoinSupportChatResponse.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory JoinSupportChatResponse.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'JoinSupportChatResponse', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  JoinSupportChatResponse clone() => JoinSupportChatResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  JoinSupportChatResponse copyWith(void Function(JoinSupportChatResponse) updates) => super.copyWith((message) => updates(message as JoinSupportChatResponse)) as JoinSupportChatResponse;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static JoinSupportChatResponse create() => JoinSupportChatResponse._();
  JoinSupportChatResponse createEmptyInstance() => create();
  static $pb.PbList<JoinSupportChatResponse> createRepeated() => $pb.PbList<JoinSupportChatResponse>();
  @$core.pragma('dart2js:noInline')
  static JoinSupportChatResponse getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<JoinSupportChatResponse>(create);
  static JoinSupportChatResponse? _defaultInstance;
}

class SupportChatMessage extends $pb.GeneratedMessage {
  factory SupportChatMessage({
    $fixnum.Int64? id,
    $core.String? name,
    MessageType? messageType,
    SupportChatMessageBase? message,
  }) {
    final $result = create();
    if (id != null) {
      $result.id = id;
    }
    if (name != null) {
      $result.name = name;
    }
    if (messageType != null) {
      $result.messageType = messageType;
    }
    if (message != null) {
      $result.message = message;
    }
    return $result;
  }
  SupportChatMessage._() : super();
  factory SupportChatMessage.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory SupportChatMessage.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'SupportChatMessage', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aInt64(1, _omitFieldNames ? '' : 'id')
    ..aOS(2, _omitFieldNames ? '' : 'name')
    ..e<MessageType>(3, _omitFieldNames ? '' : 'messageType', $pb.PbFieldType.OE, defaultOrMaker: MessageType.Notification, valueOf: MessageType.valueOf, enumValues: MessageType.values)
    ..aOM<SupportChatMessageBase>(4, _omitFieldNames ? '' : 'message', subBuilder: SupportChatMessageBase.create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessage clone() => SupportChatMessage()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessage copyWith(void Function(SupportChatMessage) updates) => super.copyWith((message) => updates(message as SupportChatMessage)) as SupportChatMessage;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static SupportChatMessage create() => SupportChatMessage._();
  SupportChatMessage createEmptyInstance() => create();
  static $pb.PbList<SupportChatMessage> createRepeated() => $pb.PbList<SupportChatMessage>();
  @$core.pragma('dart2js:noInline')
  static SupportChatMessage getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<SupportChatMessage>(create);
  static SupportChatMessage? _defaultInstance;

  @$pb.TagNumber(1)
  $fixnum.Int64 get id => $_getI64(0);
  @$pb.TagNumber(1)
  set id($fixnum.Int64 v) { $_setInt64(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasId() => $_has(0);
  @$pb.TagNumber(1)
  void clearId() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get name => $_getSZ(1);
  @$pb.TagNumber(2)
  set name($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasName() => $_has(1);
  @$pb.TagNumber(2)
  void clearName() => $_clearField(2);

  @$pb.TagNumber(3)
  MessageType get messageType => $_getN(2);
  @$pb.TagNumber(3)
  set messageType(MessageType v) { $_setField(3, v); }
  @$pb.TagNumber(3)
  $core.bool hasMessageType() => $_has(2);
  @$pb.TagNumber(3)
  void clearMessageType() => $_clearField(3);

  @$pb.TagNumber(4)
  SupportChatMessageBase get message => $_getN(3);
  @$pb.TagNumber(4)
  set message(SupportChatMessageBase v) { $_setField(4, v); }
  @$pb.TagNumber(4)
  $core.bool hasMessage() => $_has(3);
  @$pb.TagNumber(4)
  void clearMessage() => $_clearField(4);
  @$pb.TagNumber(4)
  SupportChatMessageBase ensureMessage() => $_ensure(3);
}

class SupportChatMessageBase extends $pb.GeneratedMessage {
  factory SupportChatMessageBase({
    $core.String? role,
    $core.String? text,
    $core.Iterable<FileInformation>? files,
  }) {
    final $result = create();
    if (role != null) {
      $result.role = role;
    }
    if (text != null) {
      $result.text = text;
    }
    if (files != null) {
      $result.files.addAll(files);
    }
    return $result;
  }
  SupportChatMessageBase._() : super();
  factory SupportChatMessageBase.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory SupportChatMessageBase.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'SupportChatMessageBase', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'role')
    ..aOS(2, _omitFieldNames ? '' : 'text')
    ..pc<FileInformation>(3, _omitFieldNames ? '' : 'files', $pb.PbFieldType.PM, subBuilder: FileInformation.create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessageBase clone() => SupportChatMessageBase()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessageBase copyWith(void Function(SupportChatMessageBase) updates) => super.copyWith((message) => updates(message as SupportChatMessageBase)) as SupportChatMessageBase;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static SupportChatMessageBase create() => SupportChatMessageBase._();
  SupportChatMessageBase createEmptyInstance() => create();
  static $pb.PbList<SupportChatMessageBase> createRepeated() => $pb.PbList<SupportChatMessageBase>();
  @$core.pragma('dart2js:noInline')
  static SupportChatMessageBase getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<SupportChatMessageBase>(create);
  static SupportChatMessageBase? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get role => $_getSZ(0);
  @$pb.TagNumber(1)
  set role($core.String v) { $_setString(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasRole() => $_has(0);
  @$pb.TagNumber(1)
  void clearRole() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get text => $_getSZ(1);
  @$pb.TagNumber(2)
  set text($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasText() => $_has(1);
  @$pb.TagNumber(2)
  void clearText() => $_clearField(2);

  @$pb.TagNumber(3)
  $pb.PbList<FileInformation> get files => $_getList(2);
}

class SupportChatSendMessage extends $pb.GeneratedMessage {
  factory SupportChatSendMessage({
    $fixnum.Int64? groupOwnerId,
    $core.String? sessionId,
    $core.String? message,
    $core.Iterable<FileInformation>? files,
  }) {
    final $result = create();
    if (groupOwnerId != null) {
      $result.groupOwnerId = groupOwnerId;
    }
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    if (message != null) {
      $result.message = message;
    }
    if (files != null) {
      $result.files.addAll(files);
    }
    return $result;
  }
  SupportChatSendMessage._() : super();
  factory SupportChatSendMessage.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory SupportChatSendMessage.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'SupportChatSendMessage', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aInt64(1, _omitFieldNames ? '' : 'groupOwnerId')
    ..aOS(2, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..aOS(3, _omitFieldNames ? '' : 'message')
    ..pc<FileInformation>(4, _omitFieldNames ? '' : 'files', $pb.PbFieldType.PM, subBuilder: FileInformation.create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatSendMessage clone() => SupportChatSendMessage()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatSendMessage copyWith(void Function(SupportChatSendMessage) updates) => super.copyWith((message) => updates(message as SupportChatSendMessage)) as SupportChatSendMessage;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static SupportChatSendMessage create() => SupportChatSendMessage._();
  SupportChatSendMessage createEmptyInstance() => create();
  static $pb.PbList<SupportChatSendMessage> createRepeated() => $pb.PbList<SupportChatSendMessage>();
  @$core.pragma('dart2js:noInline')
  static SupportChatSendMessage getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<SupportChatSendMessage>(create);
  static SupportChatSendMessage? _defaultInstance;

  @$pb.TagNumber(1)
  $fixnum.Int64 get groupOwnerId => $_getI64(0);
  @$pb.TagNumber(1)
  set groupOwnerId($fixnum.Int64 v) { $_setInt64(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasGroupOwnerId() => $_has(0);
  @$pb.TagNumber(1)
  void clearGroupOwnerId() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get sessionId => $_getSZ(1);
  @$pb.TagNumber(2)
  set sessionId($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasSessionId() => $_has(1);
  @$pb.TagNumber(2)
  void clearSessionId() => $_clearField(2);

  @$pb.TagNumber(3)
  $core.String get message => $_getSZ(2);
  @$pb.TagNumber(3)
  set message($core.String v) { $_setString(2, v); }
  @$pb.TagNumber(3)
  $core.bool hasMessage() => $_has(2);
  @$pb.TagNumber(3)
  void clearMessage() => $_clearField(3);

  @$pb.TagNumber(4)
  $pb.PbList<FileInformation> get files => $_getList(3);
}

class FileInformation extends $pb.GeneratedMessage {
  factory FileInformation({
    $core.String? type,
    $core.String? src,
    $core.String? name,
  }) {
    final $result = create();
    if (type != null) {
      $result.type = type;
    }
    if (src != null) {
      $result.src = src;
    }
    if (name != null) {
      $result.name = name;
    }
    return $result;
  }
  FileInformation._() : super();
  factory FileInformation.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory FileInformation.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'FileInformation', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'type')
    ..aOS(2, _omitFieldNames ? '' : 'src')
    ..aOS(3, _omitFieldNames ? '' : 'name')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  FileInformation clone() => FileInformation()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  FileInformation copyWith(void Function(FileInformation) updates) => super.copyWith((message) => updates(message as FileInformation)) as FileInformation;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static FileInformation create() => FileInformation._();
  FileInformation createEmptyInstance() => create();
  static $pb.PbList<FileInformation> createRepeated() => $pb.PbList<FileInformation>();
  @$core.pragma('dart2js:noInline')
  static FileInformation getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<FileInformation>(create);
  static FileInformation? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get type => $_getSZ(0);
  @$pb.TagNumber(1)
  set type($core.String v) { $_setString(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasType() => $_has(0);
  @$pb.TagNumber(1)
  void clearType() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get src => $_getSZ(1);
  @$pb.TagNumber(2)
  set src($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasSrc() => $_has(1);
  @$pb.TagNumber(2)
  void clearSrc() => $_clearField(2);

  @$pb.TagNumber(3)
  $core.String get name => $_getSZ(2);
  @$pb.TagNumber(3)
  set name($core.String v) { $_setString(2, v); }
  @$pb.TagNumber(3)
  $core.bool hasName() => $_has(2);
  @$pb.TagNumber(3)
  void clearName() => $_clearField(3);
}

class SupportChatMessageAck extends $pb.GeneratedMessage {
  factory SupportChatMessageAck() => create();
  SupportChatMessageAck._() : super();
  factory SupportChatMessageAck.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory SupportChatMessageAck.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'SupportChatMessageAck', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessageAck clone() => SupportChatMessageAck()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SupportChatMessageAck copyWith(void Function(SupportChatMessageAck) updates) => super.copyWith((message) => updates(message as SupportChatMessageAck)) as SupportChatMessageAck;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static SupportChatMessageAck create() => SupportChatMessageAck._();
  SupportChatMessageAck createEmptyInstance() => create();
  static $pb.PbList<SupportChatMessageAck> createRepeated() => $pb.PbList<SupportChatMessageAck>();
  @$core.pragma('dart2js:noInline')
  static SupportChatMessageAck getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<SupportChatMessageAck>(create);
  static SupportChatMessageAck? _defaultInstance;
}

class LeaveSupportChatRequest extends $pb.GeneratedMessage {
  factory LeaveSupportChatRequest({
    $fixnum.Int64? groupOwnerId,
    $core.String? sessionId,
  }) {
    final $result = create();
    if (groupOwnerId != null) {
      $result.groupOwnerId = groupOwnerId;
    }
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    return $result;
  }
  LeaveSupportChatRequest._() : super();
  factory LeaveSupportChatRequest.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory LeaveSupportChatRequest.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'LeaveSupportChatRequest', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aInt64(1, _omitFieldNames ? '' : 'groupOwnerId')
    ..aOS(2, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  LeaveSupportChatRequest clone() => LeaveSupportChatRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  LeaveSupportChatRequest copyWith(void Function(LeaveSupportChatRequest) updates) => super.copyWith((message) => updates(message as LeaveSupportChatRequest)) as LeaveSupportChatRequest;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static LeaveSupportChatRequest create() => LeaveSupportChatRequest._();
  LeaveSupportChatRequest createEmptyInstance() => create();
  static $pb.PbList<LeaveSupportChatRequest> createRepeated() => $pb.PbList<LeaveSupportChatRequest>();
  @$core.pragma('dart2js:noInline')
  static LeaveSupportChatRequest getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<LeaveSupportChatRequest>(create);
  static LeaveSupportChatRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $fixnum.Int64 get groupOwnerId => $_getI64(0);
  @$pb.TagNumber(1)
  set groupOwnerId($fixnum.Int64 v) { $_setInt64(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasGroupOwnerId() => $_has(0);
  @$pb.TagNumber(1)
  void clearGroupOwnerId() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.String get sessionId => $_getSZ(1);
  @$pb.TagNumber(2)
  set sessionId($core.String v) { $_setString(1, v); }
  @$pb.TagNumber(2)
  $core.bool hasSessionId() => $_has(1);
  @$pb.TagNumber(2)
  void clearSessionId() => $_clearField(2);
}

class LeaveSupportChatResponse extends $pb.GeneratedMessage {
  factory LeaveSupportChatResponse() => create();
  LeaveSupportChatResponse._() : super();
  factory LeaveSupportChatResponse.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory LeaveSupportChatResponse.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'LeaveSupportChatResponse', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  LeaveSupportChatResponse clone() => LeaveSupportChatResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  LeaveSupportChatResponse copyWith(void Function(LeaveSupportChatResponse) updates) => super.copyWith((message) => updates(message as LeaveSupportChatResponse)) as LeaveSupportChatResponse;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static LeaveSupportChatResponse create() => LeaveSupportChatResponse._();
  LeaveSupportChatResponse createEmptyInstance() => create();
  static $pb.PbList<LeaveSupportChatResponse> createRepeated() => $pb.PbList<LeaveSupportChatResponse>();
  @$core.pragma('dart2js:noInline')
  static LeaveSupportChatResponse getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<LeaveSupportChatResponse>(create);
  static LeaveSupportChatResponse? _defaultInstance;
}

class DisconnectRequest extends $pb.GeneratedMessage {
  factory DisconnectRequest({
    $core.String? sessionId,
  }) {
    final $result = create();
    if (sessionId != null) {
      $result.sessionId = sessionId;
    }
    return $result;
  }
  DisconnectRequest._() : super();
  factory DisconnectRequest.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory DisconnectRequest.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'DisconnectRequest', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'sessionId', protoName: 'sessionId')
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  DisconnectRequest clone() => DisconnectRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  DisconnectRequest copyWith(void Function(DisconnectRequest) updates) => super.copyWith((message) => updates(message as DisconnectRequest)) as DisconnectRequest;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static DisconnectRequest create() => DisconnectRequest._();
  DisconnectRequest createEmptyInstance() => create();
  static $pb.PbList<DisconnectRequest> createRepeated() => $pb.PbList<DisconnectRequest>();
  @$core.pragma('dart2js:noInline')
  static DisconnectRequest getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<DisconnectRequest>(create);
  static DisconnectRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get sessionId => $_getSZ(0);
  @$pb.TagNumber(1)
  set sessionId($core.String v) { $_setString(0, v); }
  @$pb.TagNumber(1)
  $core.bool hasSessionId() => $_has(0);
  @$pb.TagNumber(1)
  void clearSessionId() => $_clearField(1);
}

class DisconnectResponse extends $pb.GeneratedMessage {
  factory DisconnectResponse() => create();
  DisconnectResponse._() : super();
  factory DisconnectResponse.fromBuffer($core.List<$core.int> i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromBuffer(i, r);
  factory DisconnectResponse.fromJson($core.String i, [$pb.ExtensionRegistry r = $pb.ExtensionRegistry.EMPTY]) => create()..mergeFromJson(i, r);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(_omitMessageNames ? '' : 'DisconnectResponse', package: const $pb.PackageName(_omitMessageNames ? '' : 'support_chat'), createEmptyInstance: create)
    ..hasRequiredFields = false
  ;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  DisconnectResponse clone() => DisconnectResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  DisconnectResponse copyWith(void Function(DisconnectResponse) updates) => super.copyWith((message) => updates(message as DisconnectResponse)) as DisconnectResponse;

  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static DisconnectResponse create() => DisconnectResponse._();
  DisconnectResponse createEmptyInstance() => create();
  static $pb.PbList<DisconnectResponse> createRepeated() => $pb.PbList<DisconnectResponse>();
  @$core.pragma('dart2js:noInline')
  static DisconnectResponse getDefault() => _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<DisconnectResponse>(create);
  static DisconnectResponse? _defaultInstance;
}


const _omitFieldNames = $core.bool.fromEnvironment('protobuf.omit_field_names');
const _omitMessageNames = $core.bool.fromEnvironment('protobuf.omit_message_names');
