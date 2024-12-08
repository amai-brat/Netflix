import "/src/Pages/Shared/SupportChat/Styles/SupportChatMessage.css"
import SupportChatFiles from "./SupportChatFiles.jsx";

const SupportChatMessage = ({message}) => {
    return(
        <div className={"support-chat-message " + message.role}>
            <label style={{fontSize: "12px", textAlign: (message.role === "support" ? "left": "right")}}>{message.role === "support" ? "Поддержка" : "Вы"}</label>
            <SupportChatFiles files={message.files}/>
            <label>{message.text.trim()}</label>
        </div>
    )
}

export default SupportChatMessage