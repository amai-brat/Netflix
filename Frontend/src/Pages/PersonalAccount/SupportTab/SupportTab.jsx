import {observer} from "mobx-react";
import {useEffect, useState} from "react";
import {authenticationService} from "../../../services/authentication.service.js";
import "/src/Pages/PersonalAccount/SupportTab/Styles/SupportTab.css";
import UsersPanel from "./UsersPanel.jsx";
import SupportChatPanel from "./SupportChatPanel.jsx";
import {supportService} from "../../../services/support.service.js";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";

const SupportTab = observer(({wrapObj}) => {
    const store = useDataStore();
    const [usersMessages, setUsersMessages] = useState(undefined)
    
    useEffect(() => {
        if(!authenticationService.isCurrentUserSupport())
            return;
        
        const getSupportUsersUnansweredMessagesHistory = async () => {
            try{
                const {response, data} = await supportService.getSupportUsersUnansweredMessagesHistory();
                if(response.ok){
                    setUsersMessages(data.map((elem) => ({...elem, name:elem.userName, messages: elem.chatMessages})))
                }else{
                    setUsersMessages(null)
                }
            }
            catch (error){
                setUsersMessages(null)
            }
        }
        getSupportUsersUnansweredMessagesHistory().then();
        
    }, [store.data.chatSession]);
    
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