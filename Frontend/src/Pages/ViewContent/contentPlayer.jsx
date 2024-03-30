﻿import styles from './styles/contentPlayer.module.css';
import React, {useEffect, useState} from 'react';
import ReactPlayer from 'react-player';
import gif from './Images/loading-loading-forever.gif'
const contentPlayer = ({contentId, contentType, seasonInfos}) => {
    const [resolution, setResolution] = useState(720)
    const [error, setError] = useState(null)
    const [dataFetching, setDataFetching] = useState(false)
    const [currentEpisode, setCurrentEpisode] = useState(1)
    const [currentSeason, setCurrentSeason] = useState(1)
    const getUrl = () => {
        // TODO: написать url на сервер правильный
        return "http://localhost:5001/video/" + contentId + "?res=" + resolution +
            (contentType === "сериал"? `&episode=${currentEpisode}&season=${currentSeason}` : ``)
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
    }, [resolution, currentEpisode, currentSeason]);
    return (
        <>
            {dataFetching && <img src={gif} alt="грузится" className={styles.loading}></img>}
            {error && <div className={styles.error}>
                {error}
            </div>}
            {error == null && !dataFetching &&
                <>
                    <div className={styles.playerWindow}>
                        <div className={styles.player}>
                            <div className={styles.resAndSeasons}>
                                {contentType === "сериал" &&
                                    <div className={styles.episodeSettings}>
                                        <select value={currentEpisode} onChange={(e) => {
                                            const selectedOption = e.target.value.split('-');
                                            setCurrentSeason(parseInt(selectedOption[0]));
                                            setCurrentEpisode(parseInt(selectedOption[1]));
                                        }}>
                                            {seasonInfos.map(season => {
                                                return (
                                                    <optgroup key={season.id} label={"Сезон " + (season.seasonNumber)} >
                                                        {season.episodes.map(episode => {
                                                            return (
                                                                <option 
                                                                    key={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                                    value={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                                    className={currentEpisode === episode.episodeNumber && currentSeason === season.seasonNumber?
                                                                                styles.active : ""}>
                                                                    {episode.episodeNumber}
                                                                </option>
                                                            )
                                                        })}
                                                    </optgroup>
                                                )
                                            })}
                                        </select>
                                    </div>
                                }
                                <div className={styles.resolutionSettings}>
                                    <select value={resolution}  onChange={e => setResolution(parseInt(e.target.value))}>
                                        <option>360p</option>
                                        <option>480p</option>
                                        <option>720p</option>
                                        <option>1080p</option>
                                    </select>
                                </div>
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