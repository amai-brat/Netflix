import audioIcon from "/src/assets/AudioIcon.svg";
import videoIcon from "/src/assets/VideoIcon.svg";
import imageIcon from "/src/assets/ImageIcon.svg";
import fileIcon from "/src/assets/FileIcon.svg";
import "/src/Pages/Shared/SupportChat/Styles/SupportChatFileTypesPopUp.css";

const SupportChatFileTypesPopUp = ({setFiles, setPopUpDisplayed}) => {
    const fileTypes = [
        {type: "Картинка", accept: ".jpeg, .png, .gif, .svg", icon: imageIcon},
        {type: "Видео", accept: ".mp4, .webm, .avi, .mkv", icon: videoIcon},
        {type: "Аудио", accept: ".mp3, .wav, .aac", icon: audioIcon},
        {type: "Файл", accept: "", icon: fileIcon},
    ]

    const onFileChange = (e) => {
        const selectedFiles = Array.from(e.target.files);
        setFiles(selectedFiles);
        setPopUpDisplayed(false)
    };
    
    return (
        <div className ="support-chat-files-types-pop-up">
            {fileTypes.map((type) =>
                <div className="support-chat-file-type-wrap" key={type.type}>
                    <input className="support-chat-file-type-files-input" type="file" accept={type.accept} multiple onChange={onFileChange}/>
                    <button className="support-chat-file-type-upload-button">
                        <img className="support-chat-file-type-upload-button-icon" src={type.icon} alt="Upload"/>
                        <label>{type.type}</label>
                    </button>
                </div>
            )}
        </div>
    )
}

export default SupportChatFileTypesPopUp;