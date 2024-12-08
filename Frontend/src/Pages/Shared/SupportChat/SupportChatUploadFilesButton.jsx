import uploadIcon from "/src/assets/UploadIcon.svg";
import "/src/Pages/Shared/SupportChat/Styles/SupportChatUploadFilesButton.css";

const SupportChatUploadFilesButton = () => {
    return (
        <button className="support-chat-upload-button">
            <img className="support-chat-upload-button-icon" src={uploadIcon} alt="Upload"/>
        </button>
    )
}

export default SupportChatUploadFilesButton;