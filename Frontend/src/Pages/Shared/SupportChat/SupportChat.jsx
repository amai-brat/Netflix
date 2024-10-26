import "/src/Pages/Shared/SupportChat/Styles/SupportChat.css"
import chatIcon from "/src/assets/ChatIcon.svg"
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx";
import SupportChatPopUp from "./SupportChatPopUp.jsx";
import {useEffect, useState} from "react";
import {supportService} from "../../../services/support.service.js";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {authenticationService} from "../../../services/authentication.service.js";
import {observer} from "mobx-react";

const SupportChat = observer(() => {
    const id = "pop-up-support-chat"
    const store = useDataStore()
    const [messages, setMessages] = useState(undefined)
    const SupportChatButton = () => {
        return(
            <button id="support-chat-btn">
                <img id="support-chat-btn-icon" src={chatIcon} alt="Chat"/>
            </button>
        )
    }
    useEffect(() => {
        if (authenticationService.isCurrentUserSupport()){
            return
        }
        let isChatOk = false
        const getUserSupportMessagesHistoryAsync = async () => {
            try{
                const {response, data} = await supportService.getUserSupportMessagesHistory();
                if(response.ok){
                    setMessages(data)
                    isChatOk = true
                }else{
                    setMessages(null)
                }
            }
            catch (error){
                setMessages(null)
            }
        }
        
        getUserSupportMessagesHistoryAsync().then(() => {
            if(isChatOk){
                while (!store.data.supportConnection) setTimeout(100);
                store.data.supportConnection.on("ReceiveMessage", (supportMessage) => {
                    setMessages(messages => [...messages, supportMessage.message])
                });
            }
        })
    }, []);
    
    return(
        <div id="support-chat-block">
            {!authenticationService.isCurrentUserSupport() && <ComponentWithPopUp
                Component={() => <SupportChatButton/>}
                PopUp={({setPopUpDisplayed}) => <SupportChatPopUp 
                    setPopUpDisplayed={setPopUpDisplayed}
                    messages={messages}
                    setMessages={setMessages}
                />}
                id={id}
            />}
        </div>
    )
})

export default SupportChat