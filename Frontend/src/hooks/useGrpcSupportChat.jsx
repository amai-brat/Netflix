import {useEffect} from 'react';
import {useDataStore} from '../store/dataStoreProvider';
import {GrpcSupportChatService} from '../httpClient/grpcClient';
import {authenticationService} from '../services/authentication.service';
import {supportService} from "../services/support.service.js";

export const useGrpcSupportChat = (chatDetails, chatMessages) => {
    const {userId, role, initHistoryGroupId} = chatDetails;
    const {setHistoryMessages, setAddedMessage, setIncomingMessage, setErrorMessage} = chatMessages;
    const store = useDataStore();
    
    const getMetadata = async () => {
        const accessToken = await authenticationService.refreshTokenIfNotExpired();
        return { authorization: `Bearer ${accessToken}` };
    }

    const errorMessage = (message) => {
        setErrorMessage({
            text: message,
            role: store.data.chatSession.role
        });
    }
    
    const connect = async () => {
        const metadata = await getMetadata();
        try {
            const response = await GrpcSupportChatService.connect(metadata);
            const session = {
                id: response.getSessionid(),
                metadata: getMetadata,
                userId: userId,
                role: role
            };
            
            session.stream = GrpcSupportChatService.connectToStream(
                session.id,
                metadata,
                message => handleIncomingMessage(message)
            );
            
            store.setChatSession(session);
        } catch (error) {
            errorMessage("Не удалось подключиться");
        }
    };
    
    const disconnect = async () => {
        try {
            const session = store.data.chatSession;
            if (session) {
                if (session.stream){
                    session.stream.cancel();
                }

                const metadata = await store.data.metadata();
                await GrpcSupportChatService.disconnect(session.id, metadata);

                store.removeSession();
            }
        } catch {
            errorMessage("Что-то не так")
        }
    }
    
    const joinChat = async (groupId) => {
        try{
            const session = store.data.chatSession;
            const metadata = await store.data.metadata();
            await GrpcSupportChatService.joinSupportChat(session.id, groupId, metadata);
            await loadHistory(groupId);
        }catch{
            errorMessage("Не удалось подключиться к чату")
        }
    }

    const leaveChat = async (groupId) => {
        try{
            const session = store.data.chatSession;
            const metadata = await store.data.metadata();
            await GrpcSupportChatService.leaveSupportChat(session.id, groupId, metadata);
        }catch{
            errorMessage("Не удалось отключиться")
        }
    }

    const loadHistory = async (groupId) => {
        try {
            const userId = store.data.chatSession.userId;
            const {response, data} = userId === groupId ? 
                await supportService.getUserSupportMessagesHistory() : 
                await supportService.getSupportUserMessagesHistory(groupId); 

            if(!response.ok){
                errorMessage("Не удалось загрузить историю")
            }else{
                setHistoryMessages(data);
            }
        } catch (err) {
            errorMessage("Не удалось загрузить историю")
        }
    };
    
    const sendMessage = async (groupId, text, files = []) => {
        try {
            const uploadedFiles = files.length > 0 ? await uploadFiles(groupId, files) : [];
            const session = store.data.chatSession;
            const metadata = await session.metadata();

            if(uploadedFiles === undefined)
                return;
            
            const textToSend = (text !== null && text.trim() !== "") ? text : "";
            
            await GrpcSupportChatService.sendMessage(session.id, groupId, textToSend, uploadedFiles, metadata);
            setAddedMessage({text: textToSend, files: files, role: session.role});
        } catch (err) {
            errorMessage('Ошибка отправки сообщения');
        }
    };
    
    const uploadFiles = async (groupId, files) => {
        const formData = new FormData();
        files.forEach(file => formData.append('files', file));

        try {
            const { response, data } = await supportService.uploadChatFiles(
                groupId,
                formData
            );

            if (!response.ok){
                errorMessage("Не удалось выгрузить файлы")   
            } else {
                return data.map((url, i) => ({
                    src: url,
                    type: files[i].type,
                    name: files[i].name
                }));
            }
        } catch (err) {
            errorMessage("Не удалось выгрузить файлы")
        }
        
        return undefined;
    };
    
    const handleIncomingMessage = (message) => {
        setIncomingMessage(message)
    };

    useEffect(() => {
        if (userId && role) {
            connect().then(() => {
                if (initHistoryGroupId)
                {
                    loadHistory(initHistoryGroupId).then()   
                }
            });
        }
        return () => {
            disconnect().then();
        };
    }, [userId, role]);
    
    return{
        joinChat,
        leaveChat,
        sendMessage
    }
};