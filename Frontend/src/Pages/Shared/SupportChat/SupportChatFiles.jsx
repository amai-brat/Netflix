import downloadFileIcon from "/src/assets/DownloadFileIcon.svg";
import "/src/Pages/Shared/SupportChat/Styles/SupportChatFiles.css";
import {useRef, useState} from "react";
import ReactDOM from "react-dom";
const SupportChatFiles = ({ files }) => {
    const fileUrls = useRef({});
    
    const ImageModal = ({ src }) => {
        const [isOpen, setIsOpen] = useState(false);

        const handleImageClick = () => {
            setIsOpen(!isOpen);
        };

        return (
            <div>
                <img
                    src={src}
                    alt=""
                    className="support-chat-message-files-block-file-image"
                    onClick={handleImageClick}
                />
                {isOpen && ReactDOM.createPortal(
                    <div className="support-chat-message-files-block-file-image-modal" onClick={handleImageClick}>
                        <img src={src} alt="" className="support-chat-message-files-block-file-image-modal-image"/>
                    </div>
                ,document.body)}
            </div>
        );
    };

    return (
        <div className="support-chat-message-files-block">
            {files && files.length > 0 &&
                files.map((file, index) => {
                    let fileSrc = file.src ? file.src : URL.createObjectURL(file);
                    
                    if(fileUrls.current.hasOwnProperty(file.name)){
                        fileSrc = fileUrls.current[file.name];
                    }else{
                        fileUrls.current[file.name] = fileSrc;
                    }
                    
                    const shortFileName = file.name ?
                        file.name.length > 10 ? file.name.slice(0, 5) + "..." + file.name.slice(-6) : file.name
                        : "file";

                    return (
                        <div className="support-chat-message-files-block-file" key={index}>
                            {file && file.type.includes("image") &&
                                <ImageModal src={fileSrc}/>}
                            {file && file.type.includes("video") &&
                                <video className="support-chat-message-files-block-file-video" controls src={fileSrc}></video>}
                            {file && file.type.includes("audio") &&
                                <audio className="support-chat-message-files-block-file-audio" controls src={fileSrc}></audio>}
                            {file && !file.type.includes("video") && !file.type.includes("image") && !file.type.includes("audio") &&
                                <a className="support-chat-message-files-block-file-file" href={fileSrc} title={file.name} download={file.name ? file.name : "file"}>
                                    <img className="support-chat-message-files-block-file-file-icon" src={downloadFileIcon} alt="Download"/>
                                    <label className="support-chat-message-files-block-file-file-label">{shortFileName}</label>
                                </a>}
                        </div>
                    )
                })
            }
        </div>
    );
}

export default SupportChatFiles;