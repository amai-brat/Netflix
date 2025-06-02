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

import 'package:protobuf/protobuf.dart' as $pb;

class MessageType extends $pb.ProtobufEnum {
  static const MessageType Notification = MessageType._(0, _omitEnumNames ? '' : 'Notification');
  static const MessageType Message = MessageType._(1, _omitEnumNames ? '' : 'Message');

  static const $core.List<MessageType> values = <MessageType> [
    Notification,
    Message,
  ];

  static final $core.List<MessageType?> _byValue = $pb.ProtobufEnum.$_initByValueList(values, 1);
  static MessageType? valueOf($core.int value) =>  value < 0 || value >= _byValue.length ? null : _byValue[value];

  const MessageType._(super.v, super.n);
}


const _omitEnumNames = $core.bool.fromEnvironment('protobuf.omit_enum_names');
