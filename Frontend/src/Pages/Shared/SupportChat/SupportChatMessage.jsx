import "/src/Pages/Shared/SupportChat/Styles/SupportChatMessage.css"

const SupportChatMessage = ({message}) => {
    return(
        <div className={"message " + message.role}>
            <label>{message.text.trim()}</label>
        </div>
    )
}

export default SupportChatMessage