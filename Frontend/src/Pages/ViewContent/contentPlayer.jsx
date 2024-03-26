import styles from './styles/contentPlayer.module.css';
import React, {useState} from 'react';
import ReactPlayer from 'react-player';

const contentPlayer = ({contentId}) => {
    const [resolution, setResolution] = useState(720)
    const getUrl = () => {
        // TODO: написать url на сервер правильный
        return "http://localhost:8030/video/" + contentId + "?res=" + resolution
    }
    
    // TODO: авторизация
    return (
        <>
            <div className={styles.playerWindow}>
                <div className={styles.player}>
                    <div className={styles.resolutionSettings}>
                        <select>
                            <option onClick={() => setResolution(360)}>360p</option>
                            <option onClick={() => setResolution(480)}>480p</option>
                            <option selected={true} onClick={() => setResolution(720)}>720p</option>
                            <option onClick={() => setResolution(1080)}>1080p</option>
                        </select>
                    </div>
                    <ReactPlayer
                        url={getUrl()}
                        id="videoPlayer"
                        controls={true}
                        height={"720px"}
                        width={"1280px"}>
                    </ReactPlayer>
                </div>
            </div>
        </>
    );
}
export default contentPlayer;