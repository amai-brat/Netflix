import "/src/Pages/Shared/SupportChat/Styles/SupportChatPopUp.css"
import sendIcon from "/src/assets/SendIcon.svg"
import cross from "/src/assets/Cross.svg"
import {useEffect, useRef, useState} from "react";
import SupportChatMessage from "./SupportChatMessage.jsx";
const SupportChatPopUp = ({setPopUpDisplayed}) => {
    const endOfMessagesRef = useRef(null);
    const [messages, setMessages] = useState([])
    const [messageInput, setMessageInput] = useState("")
    const closePopUp = () => {
        setPopUpDisplayed(false)
    }
    
    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    const onSendMessageInput = () => {
        if(messageInput !== null && messageInput.trim() !== ""){
            setMessages([...messages, {text: messageInput, role: "user"}])
            setMessageInput("")
        }
    }

    useEffect(() => {
        if (endOfMessagesRef.current) {
            endOfMessagesRef.current.scrollIntoView({ behavior: 'smooth' });
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
                {messages !== undefined && messages !== null && messages.map((message) => <SupportChatMessage message={message}/>)}
                <div ref={endOfMessagesRef}/>
            </div>
            <div id="support-chat-input">
                <input type="text" id="support-chat-message-input" 
                       value={messageInput} 
                       placeholder="Введите сообщение..." 
                       onChange={onMessageInputChange} 
                       onKeyUp={(e) => {
                           if(e.key === "Enter") {
                               onSendMessageInput()
                           }
                       }}
                       
                />
                <button id="support-chat-send-button" onClick={onSendMessageInput}>
                    <img id="support-chat-send-button-icon" src={sendIcon} alt="Send"/>
                </button>
            </div>
        </div>
    )
}

export default SupportChatPopUp