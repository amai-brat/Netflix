import SupportChatFiles from "./SupportChatFiles.jsx";
import "/src/Pages/Shared/SupportChat/Styles/SupportChatFilesPreview.css";

const SupportChatFilesPreview = ({files}) => {
    return (
        <div className="support-chat-message-files-block-preview" style={{display: files && files.length > 0 ? "block" : "none"}}>
            <SupportChatFiles files={files} />
        </div>
    )
}

export default SupportChatFilesPreview