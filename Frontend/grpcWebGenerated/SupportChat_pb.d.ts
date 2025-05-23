import * as jspb from 'google-protobuf'



export class ConnectRequest extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConnectRequest.AsObject;
  static toObject(includeInstance: boolean, msg: ConnectRequest): ConnectRequest.AsObject;
  static serializeBinaryToWriter(message: ConnectRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConnectRequest;
  static deserializeBinaryFromReader(message: ConnectRequest, reader: jspb.BinaryReader): ConnectRequest;
}

export namespace ConnectRequest {
  export type AsObject = {
  }
}

export class ConnectToStreamRequest extends jspb.Message {
  getSessionid(): string;
  setSessionid(value: string): ConnectToStreamRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConnectToStreamRequest.AsObject;
  static toObject(includeInstance: boolean, msg: ConnectToStreamRequest): ConnectToStreamRequest.AsObject;
  static serializeBinaryToWriter(message: ConnectToStreamRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConnectToStreamRequest;
  static deserializeBinaryFromReader(message: ConnectToStreamRequest, reader: jspb.BinaryReader): ConnectToStreamRequest;
}

export namespace ConnectToStreamRequest {
  export type AsObject = {
    sessionid: string,
  }
}

export class ConnectResponse extends jspb.Message {
  getSessionid(): string;
  setSessionid(value: string): ConnectResponse;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConnectResponse.AsObject;
  static toObject(includeInstance: boolean, msg: ConnectResponse): ConnectResponse.AsObject;
  static serializeBinaryToWriter(message: ConnectResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConnectResponse;
  static deserializeBinaryFromReader(message: ConnectResponse, reader: jspb.BinaryReader): ConnectResponse;
}

export namespace ConnectResponse {
  export type AsObject = {
    sessionid: string,
  }
}

export class JoinSupportChatRequest extends jspb.Message {
  getGroupOwnerId(): number;
  setGroupOwnerId(value: number): JoinSupportChatRequest;

  getSessionid(): string;
  setSessionid(value: string): JoinSupportChatRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): JoinSupportChatRequest.AsObject;
  static toObject(includeInstance: boolean, msg: JoinSupportChatRequest): JoinSupportChatRequest.AsObject;
  static serializeBinaryToWriter(message: JoinSupportChatRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): JoinSupportChatRequest;
  static deserializeBinaryFromReader(message: JoinSupportChatRequest, reader: jspb.BinaryReader): JoinSupportChatRequest;
}

export namespace JoinSupportChatRequest {
  export type AsObject = {
    groupOwnerId: number,
    sessionid: string,
  }
}

export class JoinSupportChatResponse extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): JoinSupportChatResponse.AsObject;
  static toObject(includeInstance: boolean, msg: JoinSupportChatResponse): JoinSupportChatResponse.AsObject;
  static serializeBinaryToWriter(message: JoinSupportChatResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): JoinSupportChatResponse;
  static deserializeBinaryFromReader(message: JoinSupportChatResponse, reader: jspb.BinaryReader): JoinSupportChatResponse;
}

export namespace JoinSupportChatResponse {
  export type AsObject = {
  }
}

export class SupportChatMessage extends jspb.Message {
  getId(): number;
  setId(value: number): SupportChatMessage;

  getName(): string;
  setName(value: string): SupportChatMessage;

  getMessageType(): MessageType;
  setMessageType(value: MessageType): SupportChatMessage;

  getMessage(): SupportChatMessageBase | undefined;
  setMessage(value?: SupportChatMessageBase): SupportChatMessage;
  hasMessage(): boolean;
  clearMessage(): SupportChatMessage;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SupportChatMessage.AsObject;
  static toObject(includeInstance: boolean, msg: SupportChatMessage): SupportChatMessage.AsObject;
  static serializeBinaryToWriter(message: SupportChatMessage, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SupportChatMessage;
  static deserializeBinaryFromReader(message: SupportChatMessage, reader: jspb.BinaryReader): SupportChatMessage;
}

export namespace SupportChatMessage {
  export type AsObject = {
    id: number,
    name: string,
    messageType: MessageType,
    message?: SupportChatMessageBase.AsObject,
  }
}

export class SupportChatMessageBase extends jspb.Message {
  getRole(): string;
  setRole(value: string): SupportChatMessageBase;

  getText(): string;
  setText(value: string): SupportChatMessageBase;

  getFilesList(): Array<FileInformation>;
  setFilesList(value: Array<FileInformation>): SupportChatMessageBase;
  clearFilesList(): SupportChatMessageBase;
  addFiles(value?: FileInformation, index?: number): FileInformation;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SupportChatMessageBase.AsObject;
  static toObject(includeInstance: boolean, msg: SupportChatMessageBase): SupportChatMessageBase.AsObject;
  static serializeBinaryToWriter(message: SupportChatMessageBase, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SupportChatMessageBase;
  static deserializeBinaryFromReader(message: SupportChatMessageBase, reader: jspb.BinaryReader): SupportChatMessageBase;
}

export namespace SupportChatMessageBase {
  export type AsObject = {
    role: string,
    text: string,
    filesList: Array<FileInformation.AsObject>,
  }
}

export class SupportChatSendMessage extends jspb.Message {
  getGroupOwnerId(): number;
  setGroupOwnerId(value: number): SupportChatSendMessage;

  getSessionid(): string;
  setSessionid(value: string): SupportChatSendMessage;

  getMessage(): string;
  setMessage(value: string): SupportChatSendMessage;

  getFilesList(): Array<FileInformation>;
  setFilesList(value: Array<FileInformation>): SupportChatSendMessage;
  clearFilesList(): SupportChatSendMessage;
  addFiles(value?: FileInformation, index?: number): FileInformation;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SupportChatSendMessage.AsObject;
  static toObject(includeInstance: boolean, msg: SupportChatSendMessage): SupportChatSendMessage.AsObject;
  static serializeBinaryToWriter(message: SupportChatSendMessage, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SupportChatSendMessage;
  static deserializeBinaryFromReader(message: SupportChatSendMessage, reader: jspb.BinaryReader): SupportChatSendMessage;
}

export namespace SupportChatSendMessage {
  export type AsObject = {
    groupOwnerId: number,
    sessionid: string,
    message: string,
    filesList: Array<FileInformation.AsObject>,
  }
}

export class FileInformation extends jspb.Message {
  getType(): string;
  setType(value: string): FileInformation;

  getSrc(): string;
  setSrc(value: string): FileInformation;

  getName(): string;
  setName(value: string): FileInformation;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): FileInformation.AsObject;
  static toObject(includeInstance: boolean, msg: FileInformation): FileInformation.AsObject;
  static serializeBinaryToWriter(message: FileInformation, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): FileInformation;
  static deserializeBinaryFromReader(message: FileInformation, reader: jspb.BinaryReader): FileInformation;
}

export namespace FileInformation {
  export type AsObject = {
    type: string,
    src: string,
    name: string,
  }
}

export class SupportChatMessageAck extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): SupportChatMessageAck.AsObject;
  static toObject(includeInstance: boolean, msg: SupportChatMessageAck): SupportChatMessageAck.AsObject;
  static serializeBinaryToWriter(message: SupportChatMessageAck, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): SupportChatMessageAck;
  static deserializeBinaryFromReader(message: SupportChatMessageAck, reader: jspb.BinaryReader): SupportChatMessageAck;
}

export namespace SupportChatMessageAck {
  export type AsObject = {
  }
}

export class LeaveSupportChatRequest extends jspb.Message {
  getGroupOwnerId(): number;
  setGroupOwnerId(value: number): LeaveSupportChatRequest;

  getSessionid(): string;
  setSessionid(value: string): LeaveSupportChatRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): LeaveSupportChatRequest.AsObject;
  static toObject(includeInstance: boolean, msg: LeaveSupportChatRequest): LeaveSupportChatRequest.AsObject;
  static serializeBinaryToWriter(message: LeaveSupportChatRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): LeaveSupportChatRequest;
  static deserializeBinaryFromReader(message: LeaveSupportChatRequest, reader: jspb.BinaryReader): LeaveSupportChatRequest;
}

export namespace LeaveSupportChatRequest {
  export type AsObject = {
    groupOwnerId: number,
    sessionid: string,
  }
}

export class LeaveSupportChatResponse extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): LeaveSupportChatResponse.AsObject;
  static toObject(includeInstance: boolean, msg: LeaveSupportChatResponse): LeaveSupportChatResponse.AsObject;
  static serializeBinaryToWriter(message: LeaveSupportChatResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): LeaveSupportChatResponse;
  static deserializeBinaryFromReader(message: LeaveSupportChatResponse, reader: jspb.BinaryReader): LeaveSupportChatResponse;
}

export namespace LeaveSupportChatResponse {
  export type AsObject = {
  }
}

export class DisconnectRequest extends jspb.Message {
  getSessionid(): string;
  setSessionid(value: string): DisconnectRequest;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): DisconnectRequest.AsObject;
  static toObject(includeInstance: boolean, msg: DisconnectRequest): DisconnectRequest.AsObject;
  static serializeBinaryToWriter(message: DisconnectRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): DisconnectRequest;
  static deserializeBinaryFromReader(message: DisconnectRequest, reader: jspb.BinaryReader): DisconnectRequest;
}

export namespace DisconnectRequest {
  export type AsObject = {
    sessionid: string,
  }
}

export class DisconnectResponse extends jspb.Message {
  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): DisconnectResponse.AsObject;
  static toObject(includeInstance: boolean, msg: DisconnectResponse): DisconnectResponse.AsObject;
  static serializeBinaryToWriter(message: DisconnectResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): DisconnectResponse;
  static deserializeBinaryFromReader(message: DisconnectResponse, reader: jspb.BinaryReader): DisconnectResponse;
}

export namespace DisconnectResponse {
  export type AsObject = {
  }
}

export enum MessageType { 
  NOTIFICATION = 0,
  MESSAGE = 1,
}
