import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {observer} from "mobx-react";
import {useEffect, useState} from "react";
import {authenticationService} from "../../../services/authentication.service.js";
import {supportService} from "../../../services/support.service.js";
import "/src/Pages/PersonalAccount/SupportTab/Styles/SupportTab.css";
import UsersPanel from "./UsersPanel.jsx";
import SupportChatPanel from "./SupportChatPanel.jsx";

const SupportTab = observer(({wrapObj}) => {
    const store = useDataStore()
    const [usersMessages, setUsersMessages] = useState(undefined)

    useEffect(() => {
        if(!authenticationService.isCurrentUserSupport()){
            return
        }

        let isChatOk = false
        const getSupportUsersUnansweredMessagesHistoryAsync = async () => {
            try{
                const {response, data} = await supportService.getSupportUsersUnansweredMessagesHistory();
                if(response.ok){
                    setUsersMessages(data.map((elem) => ({...elem, name:elem.userName, messages: elem.chatMessages})))
                    isChatOk = true
                }else{
                    setUsersMessages(null)
                }
            }
            catch (error){
                setUsersMessages(null)
            }
        }
        if(store.data.supportConnection !== null){
            store.data.supportConnection.invoke("LeaveAllUserSupportChat").then(() => {
                getSupportUsersUnansweredMessagesHistoryAsync().then(() => {
                    if (isChatOk) {
                        store.data.supportConnection.on("ReceiveMessage", (userMessage) => {
                            setUsersMessages(prevUsersMessages => {
                                const existingMessage = prevUsersMessages.find(userMessages => userMessages.id === userMessage.id);

                                if (!existingMessage) {
                                    return [{ id: userMessage.id, name: userMessage.name, isAnswered: false, messages: null }, ...prevUsersMessages];
                                } else {
                                    return prevUsersMessages.map(userMessages =>
                                        userMessages.id === userMessage.id
                                            ? { ...userMessages, isAnswered: false, messages: (userMessages.messages ? [...userMessages.messages, userMessage.message] : null) }
                                            : userMessages
                                    );
                                }
                            });
                        });
                    }
                })
            })
        }
    }, [store.data.supportConnection]);
    
    return(
        <div id="support-tab">
            {usersMessages && authenticationService.isCurrentUserSupport() &&
                <>
                    <UsersPanel usersMessages={usersMessages} wrapObj={wrapObj}/>
                    <SupportChatPanel usersMessages={usersMessages} wrapObj={wrapObj} setUsersMessages={setUsersMessages}/>
                </>
            }
            {usersMessages === null && <label>Что-то не так</label>}
        </div>
    )
})

export default SupportTab