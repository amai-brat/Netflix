import "/src/Pages/Shared/SupportChat/Styles/SupportChatPopUp.css"
import sendIcon from "/src/assets/SendIcon.svg"
import cross from "/src/assets/Cross.svg"
import {useEffect, useRef, useState} from "react";
import SupportChatMessage from "./SupportChatMessage.jsx";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {authenticationService} from "../../../services/authentication.service.js";
const SupportChatPopUp = ({setPopUpDisplayed, messages, setMessages}) => {
    const endOfMessagesRef = useRef(null);
    const [messageInput, setMessageInput] = useState("")
    const store = useDataStore()
    const closePopUp = () => {
        setPopUpDisplayed(false)
    }
    
    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    const onSendMessageInputAsync = async () => {
        if(messageInput !== null && messageInput.trim() !== ""){
            try {
                await store.data.supportConnection.invoke("SendMessage", +authenticationService.getUser()?.id, messageInput)
                setMessages([...messages, {text: messageInput, role: "user"}])
            } catch (e) {
                if(messages === null || messages === undefined){
                    setMessages([{text: "Не удалось отправить сообщение", role: "user"}])
                }else{
                    setMessages([...messages, {text: "Не удалось отправить сообщение", role: "user"}])
                }
            }
            setMessageInput("")
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
            <div id="support-chat-input">
                <input type="text" id="support-chat-message-input" 
                       value={messageInput} 
                       placeholder="Введите сообщение..." 
                       onChange={onMessageInputChange} 
                       onKeyUp={(e) => {
                           if(e.key === "Enter") {
                               onSendMessageInputAsync()
                           }
                       }}
                />
                <button id="support-chat-send-button" onClick={onSendMessageInputAsync}>
                    <img id="support-chat-send-button-icon" src={sendIcon} alt="Send"/>
                </button>
            </div>
        </div>
    )
}

export default SupportChatPopUp