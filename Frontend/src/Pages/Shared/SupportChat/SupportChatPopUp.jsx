import "/src/Pages/Shared/SupportChat/Styles/SupportChatPopUp.css"
import sendIcon from "/src/assets/SendIcon.svg"
import cross from "/src/assets/Cross.svg"
import {useEffect, useRef, useState} from "react";
import SupportChatMessage from "./SupportChatMessage.jsx";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {authenticationService} from "../../../services/authentication.service.js";
import {supportService} from "../../../services/support.service.js";
import SupportChatFilesPreview from "./SupportChatFilesPreview.jsx";
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx";
import SupportChatUploadFilesButton from "./SupportChatUploadFilesButton.jsx";
import SupportChatFileTypesPopUp from "./SupportChatFileTypesPopUp.jsx";

const SupportChatPopUp = ({setPopUpDisplayed, messages, setMessages}) => {
    const endOfMessagesRef = useRef(null);
    const [messageInput, setMessageInput] = useState("")
    const [files, setFiles] = useState([]);
    const store = useDataStore()
    const closePopUp = () => {
        setPopUpDisplayed(false)
    }
    
    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    const errorMessage = () => {
        if(messages === null || messages === undefined){
            setMessages([{text: "Не удалось отправить сообщение", role: "user"}])
        }else{
            setMessages([...messages, {text: "Не удалось отправить сообщение", role: "user"}])
        }
    }
    
    const onSendMessageInputAsync = async () => {
        if((messageInput !== null && messageInput.trim() !== "") || files.length > 0){
            try {
                const userId = +authenticationService.getUser()?.id
                const filesDto = []
                
                if(files.length > 0){
                    const formData = new FormData();

                    files.forEach(file => {
                        formData.append("files", file);
                    });

                    const {response, data} = await supportService.uploadChatFiles(formData);
                    if (response.ok){
                        data.forEach((url, index) => {
                            filesDto.push({src: url, type: files[index].type, name: files[index].name})
                        })
                    }else{
                        errorMessage()
                        return
                    }
                }
                
                const messageInputToSend = (messageInput !== null && messageInput.trim() !== "") ? messageInput : "";

                await store.data.supportConnection.invoke("SendMessage", userId, messageInputToSend, filesDto)
                setMessages([...messages, {text: messageInput, files: filesDto, role: "user"}])
            } catch (e) {
                errorMessage()
            }
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
                    <textarea id="support-chat-message-input"
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
        </div>
    )
}

export default SupportChatPopUp