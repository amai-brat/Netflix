import "/src/Pages/PersonalAccount/SupportTab/Styles/UsersPanel.css"
import {useEffect, useRef} from "react";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import sendIcon from "../../../assets/SendIcon.svg";
import {supportService} from "../../../services/support.service.js";
import SupportChatFiles from "../../Shared/SupportChat/SupportChatFiles.jsx";
import {authenticationService} from "../../../services/authentication.service.js";
import SupportChatFilesPreview from "../../Shared/SupportChat/SupportChatFilesPreview.jsx";
import ComponentWithPopUp from "../../Shared/PopUpModule/ComponentWithPopUp.jsx";
import SupportChatUploadFilesButton from "../../Shared/SupportChat/SupportChatUploadFilesButton.jsx";
import SupportChatFileTypesPopUp from "../../Shared/SupportChat/SupportChatFileTypesPopUp.jsx";

const SupportChatPanel = ({usersMessages, setUsersMessages, wrapObj}) => {
    const files = wrapObj.files
    const setFiles = wrapObj.setFiles
    const selectedUserId = wrapObj.selectedUserId
    const user = selectedUserId !== null ? usersMessages.find((userMessages) => userMessages.id === selectedUserId) : {}
    const endOfMessagesRef = useRef(null);
    const messageInput = wrapObj.messageInput
    const setMessageInput = wrapObj.setMessageInput
    const store = useDataStore()

    const onMessageInputChange = (e) => {
        setMessageInput(e.target.value)
    }
    
    const setUsersMessagesHelp = (text, isAnswered, withNew = false, files=[] ) => {
        if(withNew){
            setUsersMessages(usersMessages =>
                usersMessages.map(userMessages =>
                    userMessages.id === selectedUserId
                        ? { ...userMessages, isAnswered: isAnswered, messages: [{text:text, files: files, role:"support"}] }
                        : userMessages
                )
            );
            return
        }
        
        setUsersMessages(usersMessages =>
            usersMessages.map(userMessages =>
                userMessages.id === selectedUserId
                    ? { ...userMessages, isAnswered: isAnswered, messages: [...userMessages.messages, {text: text, files: files, role: "support"}] }
                    : userMessages
            )
        );
    }

    const onSendMessageInputAsync = async () => {
        if((messageInput !== null && messageInput.trim() !== "") || files.length > 0){
            try {
                const filesDto = []

                //TODO: не забыть всё раскомментировать когда будет бэк

                files.forEach((file) => {
                    filesDto.push({src: URL.createObjectURL(file), type: file.type, name: file.name})
                })//убрать
                
                /*if(files.length > 0){
                    const formData = new FormData();

                    files.forEach(file => {
                        formData.append("files", file);
                    });

                    const {response, data} = await supportService.uploadChatFiles(selectedUserId, formData);
                    if (response.ok){
                        data.forEach((url, index) => {
                            filesDto.append({src: url, type: files[index].type, name: files[index].name})
                        })
                    }else{
                        setUsersMessagesHelp("Не удалось отправить сообщение", false)
                        return
                    }
                }*/
                const messageInputToSend = (messageInput !== null && messageInput.trim() !== "") ? messageInput : "";
                
                await store.data.supportConnection.invoke("SendMessage", selectedUserId, messageInputToSend)//, filesDto)
                setUsersMessagesHelp(messageInput, true, false, filesDto)
            } catch (e) {
                setUsersMessagesHelp("Не удалось отправить сообщение", false)
            }
            setMessageInput("")
            setFiles([])
        }
    }

    useEffect(() => {
        const loadHistory = async () => {
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
                    setUsersMessagesHelp("Не удалось загрузить историю сообщений", false, true)
                }
            }catch (e) {
                setUsersMessagesHelp("Не удалось загрузить историю сообщений", false, true)
            }
        }
        if(selectedUserId !== null && user.messages === null) {
            store.data.supportConnection.invoke("JoinUserSupportChat", selectedUserId).then(() => {
                loadHistory()       
            })
        }
        if (endOfMessagesRef.current) {
            endOfMessagesRef.current.scrollIntoView({ behavior: 'instant' });
        }
    }, [selectedUserId, user.messages]);
    
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