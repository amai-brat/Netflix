//
//  Generated code. Do not modify.
//  source: SupportChat.proto
//
// @dart = 3.3

// ignore_for_file: annotate_overrides, camel_case_types, comment_references
// ignore_for_file: constant_identifier_names, library_prefixes
// ignore_for_file: non_constant_identifier_names, prefer_final_fields
// ignore_for_file: unnecessary_import, unnecessary_this, unused_import

import 'dart:convert' as $convert;
import 'dart:core' as $core;
import 'dart:typed_data' as $typed_data;

@$core.Deprecated('Use messageTypeDescriptor instead')
const MessageType$json = {
  '1': 'MessageType',
  '2': [
    {'1': 'Notification', '2': 0},
    {'1': 'Message', '2': 1},
  ],
};

/// Descriptor for `MessageType`. Decode as a `google.protobuf.EnumDescriptorProto`.
final $typed_data.Uint8List messageTypeDescriptor = $convert.base64Decode(
    'CgtNZXNzYWdlVHlwZRIQCgxOb3RpZmljYXRpb24QABILCgdNZXNzYWdlEAE=');

@$core.Deprecated('Use connectRequestDescriptor instead')
const ConnectRequest$json = {
  '1': 'ConnectRequest',
};

/// Descriptor for `ConnectRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List connectRequestDescriptor = $convert.base64Decode(
    'Cg5Db25uZWN0UmVxdWVzdA==');

@$core.Deprecated('Use connectToStreamRequestDescriptor instead')
const ConnectToStreamRequest$json = {
  '1': 'ConnectToStreamRequest',
  '2': [
    {'1': 'sessionId', '3': 1, '4': 1, '5': 9, '10': 'sessionId'},
  ],
};

/// Descriptor for `ConnectToStreamRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List connectToStreamRequestDescriptor = $convert.base64Decode(
    'ChZDb25uZWN0VG9TdHJlYW1SZXF1ZXN0EhwKCXNlc3Npb25JZBgBIAEoCVIJc2Vzc2lvbklk');

@$core.Deprecated('Use connectResponseDescriptor instead')
const ConnectResponse$json = {
  '1': 'ConnectResponse',
  '2': [
    {'1': 'sessionId', '3': 1, '4': 1, '5': 9, '10': 'sessionId'},
  ],
};

/// Descriptor for `ConnectResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List connectResponseDescriptor = $convert.base64Decode(
    'Cg9Db25uZWN0UmVzcG9uc2USHAoJc2Vzc2lvbklkGAEgASgJUglzZXNzaW9uSWQ=');

@$core.Deprecated('Use joinSupportChatRequestDescriptor instead')
const JoinSupportChatRequest$json = {
  '1': 'JoinSupportChatRequest',
  '2': [
    {'1': 'group_owner_id', '3': 1, '4': 1, '5': 3, '10': 'groupOwnerId'},
    {'1': 'sessionId', '3': 2, '4': 1, '5': 9, '10': 'sessionId'},
  ],
};

/// Descriptor for `JoinSupportChatRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List joinSupportChatRequestDescriptor = $convert.base64Decode(
    'ChZKb2luU3VwcG9ydENoYXRSZXF1ZXN0EiQKDmdyb3VwX293bmVyX2lkGAEgASgDUgxncm91cE'
    '93bmVySWQSHAoJc2Vzc2lvbklkGAIgASgJUglzZXNzaW9uSWQ=');

@$core.Deprecated('Use joinSupportChatResponseDescriptor instead')
const JoinSupportChatResponse$json = {
  '1': 'JoinSupportChatResponse',
};

/// Descriptor for `JoinSupportChatResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List joinSupportChatResponseDescriptor = $convert.base64Decode(
    'ChdKb2luU3VwcG9ydENoYXRSZXNwb25zZQ==');

@$core.Deprecated('Use supportChatMessageDescriptor instead')
const SupportChatMessage$json = {
  '1': 'SupportChatMessage',
  '2': [
    {'1': 'id', '3': 1, '4': 1, '5': 3, '10': 'id'},
    {'1': 'name', '3': 2, '4': 1, '5': 9, '10': 'name'},
    {'1': 'message_type', '3': 3, '4': 1, '5': 14, '6': '.support_chat.MessageType', '10': 'messageType'},
    {'1': 'message', '3': 4, '4': 1, '5': 11, '6': '.support_chat.SupportChatMessageBase', '10': 'message'},
  ],
};

/// Descriptor for `SupportChatMessage`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List supportChatMessageDescriptor = $convert.base64Decode(
    'ChJTdXBwb3J0Q2hhdE1lc3NhZ2USDgoCaWQYASABKANSAmlkEhIKBG5hbWUYAiABKAlSBG5hbW'
    'USPAoMbWVzc2FnZV90eXBlGAMgASgOMhkuc3VwcG9ydF9jaGF0Lk1lc3NhZ2VUeXBlUgttZXNz'
    'YWdlVHlwZRI+CgdtZXNzYWdlGAQgASgLMiQuc3VwcG9ydF9jaGF0LlN1cHBvcnRDaGF0TWVzc2'
    'FnZUJhc2VSB21lc3NhZ2U=');

@$core.Deprecated('Use supportChatMessageBaseDescriptor instead')
const SupportChatMessageBase$json = {
  '1': 'SupportChatMessageBase',
  '2': [
    {'1': 'role', '3': 1, '4': 1, '5': 9, '10': 'role'},
    {'1': 'text', '3': 2, '4': 1, '5': 9, '10': 'text'},
    {'1': 'files', '3': 3, '4': 3, '5': 11, '6': '.support_chat.FileInformation', '10': 'files'},
  ],
};

/// Descriptor for `SupportChatMessageBase`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List supportChatMessageBaseDescriptor = $convert.base64Decode(
    'ChZTdXBwb3J0Q2hhdE1lc3NhZ2VCYXNlEhIKBHJvbGUYASABKAlSBHJvbGUSEgoEdGV4dBgCIA'
    'EoCVIEdGV4dBIzCgVmaWxlcxgDIAMoCzIdLnN1cHBvcnRfY2hhdC5GaWxlSW5mb3JtYXRpb25S'
    'BWZpbGVz');

@$core.Deprecated('Use supportChatSendMessageDescriptor instead')
const SupportChatSendMessage$json = {
  '1': 'SupportChatSendMessage',
  '2': [
    {'1': 'group_owner_id', '3': 1, '4': 1, '5': 3, '10': 'groupOwnerId'},
    {'1': 'sessionId', '3': 2, '4': 1, '5': 9, '10': 'sessionId'},
    {'1': 'message', '3': 3, '4': 1, '5': 9, '10': 'message'},
    {'1': 'files', '3': 4, '4': 3, '5': 11, '6': '.support_chat.FileInformation', '10': 'files'},
  ],
};

/// Descriptor for `SupportChatSendMessage`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List supportChatSendMessageDescriptor = $convert.base64Decode(
    'ChZTdXBwb3J0Q2hhdFNlbmRNZXNzYWdlEiQKDmdyb3VwX293bmVyX2lkGAEgASgDUgxncm91cE'
    '93bmVySWQSHAoJc2Vzc2lvbklkGAIgASgJUglzZXNzaW9uSWQSGAoHbWVzc2FnZRgDIAEoCVIH'
    'bWVzc2FnZRIzCgVmaWxlcxgEIAMoCzIdLnN1cHBvcnRfY2hhdC5GaWxlSW5mb3JtYXRpb25SBW'
    'ZpbGVz');

@$core.Deprecated('Use fileInformationDescriptor instead')
const FileInformation$json = {
  '1': 'FileInformation',
  '2': [
    {'1': 'type', '3': 1, '4': 1, '5': 9, '10': 'type'},
    {'1': 'src', '3': 2, '4': 1, '5': 9, '10': 'src'},
    {'1': 'name', '3': 3, '4': 1, '5': 9, '10': 'name'},
  ],
};

/// Descriptor for `FileInformation`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List fileInformationDescriptor = $convert.base64Decode(
    'Cg9GaWxlSW5mb3JtYXRpb24SEgoEdHlwZRgBIAEoCVIEdHlwZRIQCgNzcmMYAiABKAlSA3NyYx'
    'ISCgRuYW1lGAMgASgJUgRuYW1l');

@$core.Deprecated('Use supportChatMessageAckDescriptor instead')
const SupportChatMessageAck$json = {
  '1': 'SupportChatMessageAck',
};

/// Descriptor for `SupportChatMessageAck`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List supportChatMessageAckDescriptor = $convert.base64Decode(
    'ChVTdXBwb3J0Q2hhdE1lc3NhZ2VBY2s=');

@$core.Deprecated('Use leaveSupportChatRequestDescriptor instead')
const LeaveSupportChatRequest$json = {
  '1': 'LeaveSupportChatRequest',
  '2': [
    {'1': 'group_owner_id', '3': 1, '4': 1, '5': 3, '10': 'groupOwnerId'},
    {'1': 'sessionId', '3': 2, '4': 1, '5': 9, '10': 'sessionId'},
  ],
};

/// Descriptor for `LeaveSupportChatRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List leaveSupportChatRequestDescriptor = $convert.base64Decode(
    'ChdMZWF2ZVN1cHBvcnRDaGF0UmVxdWVzdBIkCg5ncm91cF9vd25lcl9pZBgBIAEoA1IMZ3JvdX'
    'BPd25lcklkEhwKCXNlc3Npb25JZBgCIAEoCVIJc2Vzc2lvbklk');

@$core.Deprecated('Use leaveSupportChatResponseDescriptor instead')
const LeaveSupportChatResponse$json = {
  '1': 'LeaveSupportChatResponse',
};

/// Descriptor for `LeaveSupportChatResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List leaveSupportChatResponseDescriptor = $convert.base64Decode(
    'ChhMZWF2ZVN1cHBvcnRDaGF0UmVzcG9uc2U=');

@$core.Deprecated('Use disconnectRequestDescriptor instead')
const DisconnectRequest$json = {
  '1': 'DisconnectRequest',
  '2': [
    {'1': 'sessionId', '3': 1, '4': 1, '5': 9, '10': 'sessionId'},
  ],
};

/// Descriptor for `DisconnectRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List disconnectRequestDescriptor = $convert.base64Decode(
    'ChFEaXNjb25uZWN0UmVxdWVzdBIcCglzZXNzaW9uSWQYASABKAlSCXNlc3Npb25JZA==');

@$core.Deprecated('Use disconnectResponseDescriptor instead')
const DisconnectResponse$json = {
  '1': 'DisconnectResponse',
};

/// Descriptor for `DisconnectResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List disconnectResponseDescriptor = $convert.base64Decode(
    'ChJEaXNjb25uZWN0UmVzcG9uc2U=');

