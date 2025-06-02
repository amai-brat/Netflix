import "/src/Pages/Shared/SupportChat/Styles/SupportChatPopUp.css"
import sendIcon from "/src/assets/SendIcon.svg"
import cross from "/src/assets/Cross.svg"
import {useEffect, useRef, useState} from "react";
import SupportChatMessage from "./SupportChatMessage.jsx";
import {authenticationService} from "../../../services/authentication.service.js";
import SupportChatFilesPreview from "./SupportChatFilesPreview.jsx";
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx";
import SupportChatUploadFilesButton from "./SupportChatUploadFilesButton.jsx";
import SupportChatFileTypesPopUp from "./SupportChatFileTypesPopUp.jsx";
import {useGrpcSupportChat} from "../../../hooks/useGrpcSupportChat.jsx";
import {observer} from "mobx-react";

/* eslint-disable no-unused-vars */
const SupportChatPopUp = observer(({setPopUpDisplayed}) => {
    const endOfMessagesRef = useRef(null);
    const [messages, setMessages] = useState(undefined)
    const [files, setFiles] = useState([]);
    const user = authenticationService.getUser()
    const chatDetails = {
        userId : user.id,
        role: "user",
        initHistoryGroupId: user.id
    };
    // noinspection JSUnusedGlobalSymbols
    const chatMessages = {
        setHistoryMessages: (history) => {setMessages(messages => [...history, ...(messages ?? [])])}, 
        setAddedMessage: (message) => {setMessages(messages => [...(messages ?? []), message])},
        setIncomingMessage: (message) => {setMessages(messages => [...(messages ?? []), message.message])},
        setErrorMessage: (message) => {setMessages(messages => [...(messages ?? []), message])},
        setLeaveMessages: () => { // noinspection JSUnusedLocalSymbols
            setMessages(messages => undefined)}
    }
    const {sendMessage} = useGrpcSupportChat(chatDetails, chatMessages);
    
    const closePopUp = () => {
        setPopUpDisplayed(false)
    }
    
    const onSendMessageInputAsync = async (messageInput, setMessageInput) => {
        if((messageInput !== null && messageInput.trim() !== "") || files.length > 0){
            await sendMessage(user.id, messageInput, files);
            setMessageInput("")
            setFiles([])
        }
    }

    useEffect(() => {
        if (endOfMessagesRef.current) {
            endOfMessagesRef.current.scrollIntoView({ behavior: 'instant' });
        }
    }, [messages]);
    
    return(
        <div id="support-chat-pop-up">
            <div id="support-chat-header">
                <label>Чат поддержки</label>
                <button id="support-chat-header-btn" onClick={() => closePopUp()}>
                    <img id="support-chat-header-btn-icon" src={cross} alt="Cross"/>
                </button>
            </div>
            <div id="support-chat-messages">
                {messages === null && <label>Что-то не так</label>}
                {messages !== undefined && messages !== null && messages.map((message, index) => 
                    <div key={index}>
                        <SupportChatMessage message={message}/>
                    </div>
                )}
                <div ref={endOfMessagesRef}/>
            </div>
            <div id="support-chat-input-wrap">
                <div id="support-chat-files-preview-wrap">
                    <SupportChatFilesPreview files={files} />
                </div>
                <div id="support-chat-input">
                    <ComponentWithPopUp
                        Component={SupportChatUploadFilesButton}
                        PopUp={({setPopUpDisplayed}) => <SupportChatFileTypesPopUp setFiles={setFiles} setPopUpDisplayed={setPopUpDisplayed}/>}
                        id="support-chat-files-types-pop-up"
                    />
                    <SupportChatInput onSendMessageInputAsync={onSendMessageInputAsync}/>
                </div>
            </div>
        </div>
    )
});

const SupportChatInput = ({onSendMessageInputAsync}) => {
    const [messageInput, setMessageInput] = useState("")
    
    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    return (
        <>
            <textarea id="support-chat-message-input"
                      value={messageInput}
                      placeholder="Введите сообщение..."
                      onChange={onMessageInputChange}
                      onKeyUp={(e) => {
                          if (e.key === "Enter") {
                              onSendMessageInputAsync(messageInput, setMessageInput)
                          }
                      }}
            />
            <button id="support-chat-send-button" onClick={() => onSendMessageInputAsync(messageInput, setMessageInput)}>
                <img id="support-chat-send-button-icon" src={sendIcon} alt="Send"/>
            </button>
        </>
    )
}

export default SupportChatPopUp