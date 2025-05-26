import "/src/Pages/PersonalAccount/SupportTab/Styles/UsersPanel.css"
import {useEffect, useRef} from "react";
import sendIcon from "../../../assets/SendIcon.svg";
import SupportChatFiles from "../../Shared/SupportChat/SupportChatFiles.jsx";
import SupportChatFilesPreview from "../../Shared/SupportChat/SupportChatFilesPreview.jsx";
import ComponentWithPopUp from "../../Shared/PopUpModule/ComponentWithPopUp.jsx";
import SupportChatUploadFilesButton from "../../Shared/SupportChat/SupportChatUploadFilesButton.jsx";
import SupportChatFileTypesPopUp from "../../Shared/SupportChat/SupportChatFileTypesPopUp.jsx";
import {useGrpcSupportChat} from "../../../hooks/useGrpcSupportChat.jsx";
import {authenticationService} from "../../../services/authentication.service.js";

const SupportChatPanel = ({usersMessages, setUsersMessages, wrapObj}) => {
    const files = wrapObj.files
    const setFiles = wrapObj.setFiles
    const selectedUserId = wrapObj.selectedUserId
    const user = selectedUserId !== null ? usersMessages.find((userMessages) => userMessages.id === selectedUserId) : {}
    const endOfMessagesRef = useRef(null);
    const messageInput = wrapObj.messageInput
    const setMessageInput = wrapObj.setMessageInput
    const chatDetails = {
        userId : authenticationService.getUser().id,
        role: "support",
        initHistoryGroupId: selectedUserId
    };
    // noinspection JSUnusedGlobalSymbols
    const chatMessages = {
        setHistoryMessages: (history) => {
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, messages: [...history, ...(userMessages.messages ?? [])] }
                        : userMessages
                )
            );
        },
        setAddedMessage: (message) => {
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, isAnswered: true, messages: [...(userMessages.messages ?? []), message] }
                        : userMessages
                )
            );
        },
        setIncomingMessage: (message) => {
            setUsersMessages(prevUsersMessages => {
                const existingMessage = prevUsersMessages.find(userMessages => userMessages.id === message.id);
                if(existingMessage && message.messageType === "notification") {
                    return prevUsersMessages;
                }
                
                if (!existingMessage) {
                    return [{ id: message.id, name: message.name, isAnswered: false, messages: null }, ...prevUsersMessages];
                } else {
                    return prevUsersMessages.map(userMessages =>
                        userMessages.id === message.id
                            ? { ...userMessages, isAnswered: false, messages: (userMessages.messages ? [...userMessages.messages, message.message] : null) }
                            : userMessages
                    );
                }
            });
        },
        setErrorMessage: (message) => {
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, isAnswered: false, messages: [...(userMessages.messages ?? []), message] }
                        : userMessages
                )
            );
        },
        setLeaveMessages: () => {
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, messages: null }
                        : userMessages
                )
            );
        }
    }
    const {joinChat, leaveChat, sendMessage} = useGrpcSupportChat(chatDetails, chatMessages)

    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }

    const onSendMessageInputAsync = async () => {
        if((messageInput !== null && messageInput.trim() !== "") || files.length > 0){
            await sendMessage(selectedUserId, messageInput, files);
            setMessageInput("")
            setFiles([])
        }
    }

    useEffect(() => {
        if(selectedUserId !== null) {
            joinChat(selectedUserId).then();
        }
        return () => {
            leaveChat(selectedUserId).then();
        }
    }, [selectedUserId]);
    
    useEffect(() => {
        if (endOfMessagesRef.current) {
            endOfMessagesRef.current.scrollIntoView({ behavior: 'instant' });
        }
    }, [user.messages]);
    
    return (
        <div id="support-tab-chat-panel">
            <div id="support-tab-chat-panel-header">
                {(selectedUserId ? <label>Чат с пользователем {user.name}</label>
                    : <label>Выберите пользователя для начала чата.</label>)}
            </div>
            {selectedUserId && user.messages && <div id="support-tab-chat-panel-aside">
                {user.messages.map((msg, index) => (
                    <div key={index} className={"support-chat-panel-message " + msg.role}>
                        <label style={{
                            fontSize: "12px",
                            textAlign: (msg.role === "support" ? "right" : "left")
                        }}>{msg.role === "support" ? "Поддержка" : "Пользователь"}</label>
                        <SupportChatFiles files={msg.files}/>
                        <label>{msg.text.trim()}</label>
                    </div>
                ))}
                <div ref={endOfMessagesRef}/>
            </div>}
            {!selectedUserId && <div id="support-tab-chat-panel-aside"></div>}
            <div id="support-tab-chat-panel-input-wrap">
                <div id="support-tab-chat-panel-files-preview-wrap">
                    <SupportChatFilesPreview files={files}/>
                </div>
                <div id="support-tab-chat-panel-input">
                    <ComponentWithPopUp
                        Component={SupportChatUploadFilesButton}
                        PopUp={({setPopUpDisplayed}) => <SupportChatFileTypesPopUp setFiles={setFiles} setPopUpDisplayed={setPopUpDisplayed}/>}
                        id="support-tab-chat-panel-files-types-pop-up"
                    />
                    <textarea id="support-tab-chat-panel-message-input"
                           value={messageInput}
                           placeholder="Введите сообщение..."
                           disabled={selectedUserId === null}
                           onChange={onMessageInputChange}
                           onKeyUp={(e) => {
                               if (e.key === "Enter") {
                                   onSendMessageInputAsync()
                               }
                           }}
                    />
                    <button id="support-tab-chat-panel-send-button" onClick={onSendMessageInputAsync}
                            disabled={selectedUserId === null}>
                        <img id="support-tab-chat-panel-send-button-icon" src={sendIcon} alt="Send"/>
                    </button>
                </div>
            </div>
        </div>
    )
}

export default SupportChatPanel