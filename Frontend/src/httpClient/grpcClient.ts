import {SupportChatClient} from "grpc-web-generated/SupportChatServiceClientPb.js";
import {
    ConnectRequest,
    ConnectResponse,
    ConnectToStreamRequest,
    DisconnectRequest,
    DisconnectResponse,
    FileInformation,
    JoinSupportChatRequest,
    JoinSupportChatResponse,
    LeaveSupportChatResponse,
    SupportChatMessageAck,
    SupportChatSendMessage,
    SupportChatMessage, 
    MessageType
} from "grpc-web-generated/SupportChat_pb";
import {baseSupportChatUrl} from "./baseUrl.js";

const client = new SupportChatClient(baseSupportChatUrl);

export const GrpcSupportChatService = {
    connect: (metadata: any) : Promise<ConnectResponse> => {
        const request = new ConnectRequest();
        return new Promise((resolve, reject) => {
            client.connect(request, metadata, (err, response) => {
                if (err) reject(err);
                resolve(response);
            });
        });
    },

    connectToStream: (sessionId: any, metadata: any, callback: any) => {
        const request = new ConnectToStreamRequest();
        request.setSessionid(sessionId);
        const stream = client.connectToStream(request, metadata);
        stream.on('data', (message: SupportChatMessage) => {
            callback(normalizeMessage(message));
        });
        return stream;
    },
    
    joinSupportChat: (sessionId: any, groupId: any, metadata: any): Promise<JoinSupportChatResponse> => {
        const request = new JoinSupportChatRequest();
        request.setSessionid(sessionId);
        request.setGroupOwnerId(groupId);
        
        return new Promise((resolve, reject) => {
            client.joinSupportChat(request, metadata, (err, response) => {
                if (err) reject(err);
                resolve(response);
            });
        });
    },
    
    leaveSupportChat: (sessionId: any, groupId: any, metadata: any): Promise<LeaveSupportChatResponse> => {
        const request = new JoinSupportChatRequest();
        request.setSessionid(sessionId);
        request.setGroupOwnerId(groupId);
        
        return new Promise((resolve, reject) => {
            client.leaveSupportChat(request, metadata, (err, response) => {
                if (err) reject(err);
                resolve(response);
            });
        });
    },

    sendMessage: (sessionId: any, groupId: any, text: any, uploadedFiles: any, metadata: any): Promise<SupportChatMessageAck> => {
        const request = new SupportChatSendMessage();

        request.setSessionid(sessionId);
        request.setGroupOwnerId(groupId);
        request.setMessage(text);
        request.setFilesList(
            uploadedFiles.map(f => {
                const file = new FileInformation();
                file.setType(f.type);
                file.setSrc(f.src);
                file.setName(f.name);
                return file;
            })
        );
        
        return new Promise((resolve, reject) => {
            client.sendMessage(request, metadata, (err, response) => {
                if (err) reject(err);
                resolve(response);
            });
        });
    },
    
    disconnect: (sessionId: any, metadata: any) : Promise<DisconnectResponse> => {
        const request = new DisconnectRequest()
        request.setSessionid(sessionId);
        
        return new Promise((resolve, reject) => {
            client.disconnect(request, metadata, (err, response) => {
                if (err) reject(err);
                resolve(response);
            });
        });
    }
};

const normalizeMessage = (message: SupportChatMessage): any => {
    let messageType = getMessageType(message.getMessageType());
    const messageBase = message.getMessage();
    return {
        id: message.getId(),
        name: message.getName(),
        messageType: messageType,
        message : {
            role: messageBase.getRole(),
            text: messageBase.getText(),
            files: messageBase.getFilesList().map((f) => {
                return {
                    src: f.getSrc(),
                    name: f.getName(),
                    type: f.getType(),
                };
            }),
        }
    };
}

const getMessageType = (messageType: MessageType): string => {
    switch (messageType) {
        case MessageType.NOTIFICATION: {
            return "notification";
        }
        case MessageType.MESSAGE: {
            return "message";
        }
        default:{
            return  "unknown";
        }
    }
}