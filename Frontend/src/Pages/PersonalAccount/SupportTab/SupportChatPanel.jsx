import "/src/Pages/PersonalAccount/SupportTab/Styles/UsersPanel.css"
import {useEffect, useRef, useState} from "react";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import sendIcon from "../../../assets/SendIcon.svg";
import {supportService} from "../../../services/support.service.js";

const SupportChatPanel = ({usersMessages, selectedUserId, setUsersMessages}) => {
    const user = selectedUserId !== null ? usersMessages.find((userMessages) => userMessages.id === selectedUserId) : {}
    const endOfMessagesRef = useRef(null);
    const [messageInput, setMessageInput] = useState("")
    const store = useDataStore()

    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    const setUsersMessagesHelp = (text, withNew = false ) => {
        if(withNew){
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, messages: [{text:text, role:"support"}] }
                        : userMessages
                )
            );
            return
        }
        
        setUsersMessages(usersMessages =>
            usersMessages.map(userMessages =>
                userMessages.id === selectedUserId
                    ? { ...userMessages, messages: [...userMessages.messages, {text: text, role: "support"}] }
                    : userMessages
            )
        );
    }

    const onSendMessageInputAsync = async () => {
        if(messageInput !== null && messageInput.trim() !== ""){
            try {
                await store.data.supportConnection.invoke("SendMessageToUserAsync", messageInput)
                setUsersMessagesHelp(messageInput)
            } catch (e) {
                setUsersMessagesHelp("Не удалось отправить сообщение")
            }
            setMessageInput("")
        }
    }

    useEffect(() => {
        const loadHistory = async () => {
            if(selectedUserId !== null && user.messages === null){
                try{
                    const {response, data} = await supportService.getSupportUserMessagesHistory(selectedUserId)
                    if(response.ok){
                        setUsersMessages(usersMessages =>
                            usersMessages.map(userMessages =>
                                userMessages.id === selectedUserId
                                    ? { ...userMessages, messages: data }
                                    : userMessages
                            )
                        );
                    }else{
                        setUsersMessagesHelp("Не удалось загрузить историю сообщений", true)
                    }
                }catch (e) {
                    setUsersMessagesHelp("Не удалось загрузить историю сообщений", true)
                }
            }
        }
        
        loadHistory().then(() => {
            if (endOfMessagesRef.current) {
                endOfMessagesRef.current.scrollIntoView({ behavior: 'smooth' });
            }  
        })
    }, [user.messages]);
    
    return (
        <div id="support-tab-chat-panel">
            <div id="support-tab-chat-panel-header">
                {(selectedUserId ? <label>Чат с пользователем {user.name}</label>
                    : <label>Выберите пользователя для начала чата.</label>)}
            </div>
            {selectedUserId && <div id="support-tab-chat-panel-aside">
                {user.messages.map((msg, index) => (
                    <div key={index} className={"support-chat-panel-message " + msg.role}>
                        <label style={{
                            fontSize: "12px",
                            textAlign: (msg.role === "support" ? "right" : "left")
                        }}>{msg.role === "support" ? "Поддержка" : "Пользователь"}</label>
                        <label>{msg.text.trim()}</label>
                    </div>
                ))}
                <div ref={endOfMessagesRef}/>
            </div>}
            {!selectedUserId && <div id="support-tab-chat-panel-aside"></div>}
            <div id="support-tab-chat-panel-input">
                <input type="text" id="support-tab-chat-panel-message-input"
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
    )
}

export default SupportChatPanel