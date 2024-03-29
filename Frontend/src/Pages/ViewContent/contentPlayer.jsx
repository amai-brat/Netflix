import styles from './styles/contentPlayer.module.css';
import React, {useEffect, useState} from 'react';
import ReactPlayer from 'react-player';
import gif from './Images/loading-loading-forever.gif'
const contentPlayer = ({contentId}) => {
    const [resolution, setResolution] = useState(720)
    const [error, setError] = useState(null)
    const [dataFetching, setDataFetching] = useState(false)
    const getUrl = () => {
        // TODO: написать url на сервер правильный
        return "http://localhost:8030/video/" + contentId + "?res=" + resolution
    }
    // этот useEffect проверяет что пользователь МОЖЕТ смотреть видео(иначе у него будет окно что нельзя)
    useEffect(() => {
        async function fetchData() {
            try {
                setDataFetching(true)
                const resp = await fetch(getUrl());
                if (!resp.ok) {
                    setError("Ошибка загрузки видео")
                }
            } catch (e) {
                setError(e.message)
                setDataFetching(false)
            } finally {
                setDataFetching(false)
            }
        }
        fetchData();
    }, [contentId, resolution]);
    return (
        <>
            {dataFetching && <img src={gif} alt="я джифка" className={styles.loading}></img>}
            {error && <div className={styles.error}>
                {error}
            </div>}
            {error == null && !dataFetching &&
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
            }
        </>
    );
}
export default contentPlayer;