import "/src/Pages/Shared/SupportChat/Styles/SupportChat.css"
import chatIcon from "/src/assets/ChatIcon.svg"
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx";
import SupportChatPopUp from "./SupportChatPopUp.jsx";
import {authenticationService} from "../../../services/authentication.service.js";

const SupportChat = () => {
    const id = "pop-up-support-chat"
    
    const SupportChatButton = () => {
        return(
            <button id="support-chat-btn">
                <img id="support-chat-btn-icon" src={chatIcon} alt="Chat"/>
            </button>
        )
    }
    
    return(
        <div id="support-chat-block">
            {!authenticationService.isCurrentUserSupport() && <ComponentWithPopUp
                Component={() => <SupportChatButton/>}
                PopUp={({setPopUpDisplayed}) => <SupportChatPopUp 
                    setPopUpDisplayed={setPopUpDisplayed}
                />}
                id={id}
                extraOpenStateOptions={[
                    (e) => !e.target.classList.contains("support-chat-message-files-block-file-image-modal"),
                    (e) => !e.target.classList.contains("support-chat-message-files-block-file-image-modal-image")
                ]}
            />}
        </div>
    )
}

export default SupportChat