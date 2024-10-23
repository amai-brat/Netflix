import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {observer} from "mobx-react";
import {useEffect, useState} from "react";
import {authenticationService} from "../../../services/authentication.service.js";
import {supportService} from "../../../services/support.service.js";
import "/src/Pages/PersonalAccount/SupportTab/Styles/SupportTab.css";
import UsersPanel from "./UsersPanel.jsx";
import SupportChatPanel from "./SupportChatPanel.jsx";

const SupportTab = observer(() => {
    const store = useDataStore()
    const [usersMessages, setUsersMessages] = useState(undefined)
    const [selectedUserId, setSelectedUserId] = useState(null);

    useEffect(() => {
        if(!authenticationService.isCurrentUserSupport()){
            return
        }

        let isChatOk = false
        const getSupportUsersUnansweredMessagesHistoryAsync = async () => {
            try{
                const {response, data} = await supportService.getSupportUsersUnansweredMessagesHistory();
                if(response.ok){
                    setUsersMessages(data)
                    isChatOk = true
                }else{
                    setUsersMessages(null)
                }
            }
            catch (error){
                setUsersMessages(null)
            }
        }

        getSupportUsersUnansweredMessagesHistoryAsync().then(() => {
            //TODO: заменить methodName если отличаeтся
            if(isChatOk){
                store.data.supportConnection.on("ReceiveUserMessage", (userMessage) => {
                    if(usersMessages.filter((userMessages) => userMessages.id === userMessage.id).length === 0){
                        setUsersMessages(usersMessages => [...usersMessages, {id: userMessage.id, name: userMessage.name, messages:[] }])
                    }else{
                        setUsersMessages(usersMessages =>
                            usersMessages.map(userMessages =>
                                userMessages.id === userMessage.id
                                    ? { ...userMessages, messages: [...userMessages.messages, userMessage.message] }
                                    : userMessages
                            )
                        );
                    }
                });
            }
        })
    }, []);
    
    return(
        <div id="support-tab">
            {usersMessages && authenticationService.isCurrentUserSupport() &&
                <>
                    <UsersPanel usersMessages={usersMessages} selectedUserId={selectedUserId} setSelectedUserId={setSelectedUserId}/>
                    <SupportChatPanel usersMessages={usersMessages} selectedUserId={selectedUserId} setUsersMessages={setUsersMessages}/>
                </>
            }
            {!usersMessages && <label>Что-то не так</label>}
        </div>
    )
})

export default SupportTab