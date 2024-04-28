import styles from './styles/contentPlayer.module.css';
import React, {useEffect, useState} from 'react';
import ReactPlayer from 'react-player';
import gif from './Images/loading-loading-forever.gif'
import {baseUrl} from "../../httpClient/baseUrl.js";
import Hls from 'hls.js';
import {get} from "mobx";
import {fetchAuth} from "../../httpClient/fetchAuth.js";
const contentPlayer = ({contentId, contentType, seasonInfos}) => {
    const [resolution, setResolution] = useState(1080)
    const [occuredError, setOccuredError] = useState(null)
    const [dataFetching, setDataFetching] = useState(false)
    const [currentEpisode, setCurrentEpisode] = useState(1)
    const [currentSeason, setCurrentSeason] = useState(1)
    const [videoUrl, setVideoUrl] = useState('');
    const maxRetries = 3;
    let retries = 0;
    // это нужно в случае если токен протухнет при просмотре видео. 3 раза пытаемся получить новый, если не получается
    // то выводим ошибку 
    const updateTokenAndRetry = async () => {
        if (retries > maxRetries){
            setOccuredError("вам нужно авторизироваться")
        }
        try{
            const {response} = await fetchAuth(getUrl())
            if (response.ok){
                retries = 0
            }
            else{
                retries++;
            }
        } catch (error){
            setOccuredError("произошла ошибка")
            retries++;
        } 
    } 
    const getUrl = (contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode) => {
        let path;
        if (contentType.contentTypeName === "Сериал") {
            // serial/{id}/season/{season}/episode/{episode}/res/{resolution}/
            path = `${baseUrl}content/serial/${contentId}/season/${currentSeason}/episode/${currentEpisode}/res/${resolution}/output.m3u8`;
        } else {
            path = `${baseUrl}content/movie/${contentId}/res/${resolution}/output.m3u8`;
        }
        return path;
    };
    // Обновляем URL каждый раз при изменении параметров
    useEffect(() => {
        setVideoUrl(getUrl(contentId, contentType, seasonInfos, resolution,currentSeason,currentEpisode));
    }, [contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode]);
    
    // этот useEffect проверяет что пользователь МОЖЕТ смотреть видео(иначе у него будет окно что нельзя)
    useEffect(() => {
        async function fetchData() {
            try {
                setDataFetching(true)
                const resp = await fetch(videoUrl, {
                    headers: {
                        "Authorization": "Bearer " + sessionStorage.getItem("accessToken")
                    }
                });
                if (!resp.ok) {
                    setOccuredError("Ошибка загрузки видео")
                    return;
                }
                setOccuredError(null)
            } catch (e) {
                setOccuredError(e.message)
                setDataFetching(false)
            } finally {
                setDataFetching(false)
            }
        }
        fetchData();
    }, [videoUrl]);
    return (
        <>
            {dataFetching && <img src={gif} alt="грузится" className={styles.loading}></img>}
            {occuredError && <div className={styles.error}>
                <div className={styles.resAndSeasons}>
                    {contentType.contentTypeName === "Сериал" &&
                        <div className={styles.episodeSettings}>
                            <select value={`${currentSeason}-${currentEpisode}`} onChange={(e) => {
                                const selectedOption = e.target.value.split('-');
                                setCurrentSeason(parseInt(selectedOption[0]));
                                setCurrentEpisode(parseInt(selectedOption[1]));
                            }}>
                                {seasonInfos.map(season => {
                                    return (
                                        <optgroup key={season.id} label={"Сезон " + (season.seasonNumber)}>
                                            {season.episodes.map(episode => {
                                                return (
                                                    <option
                                                        key={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                        value={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                        className={currentEpisode === episode.episodeNumber && currentSeason === season.seasonNumber ?
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
                        <select value={resolution}
                                onChange={e => setResolution(parseInt(e.target.value))}>
                            <option value={"360"}>360p</option>
                            <option value={"480"}>480p</option>
                            <option value={"720"}>720p</option>
                            <option value={"1080"}>1080p</option>
                        </select>
                    </div>
                </div>
                {occuredError}
            </div>}
            {occuredError == null && !dataFetching &&
                <>
                    <div className={styles.playerWindow}>
                        <div className={styles.player}>
                            <div className={styles.resAndSeasons}>
                                {contentType.contentTypeName === "Сериал" &&
                                    <div className={styles.episodeSettings}>
                                        <select value={`${currentSeason}-${currentEpisode}`} onChange={(e) => {
                                            const selectedOption = e.target.value.split('-');
                                            setCurrentSeason(parseInt(selectedOption[0]));
                                            setCurrentEpisode(parseInt(selectedOption[1]));
                                        }}>
                                            {seasonInfos.map(season => {
                                                return (
                                                    <optgroup key={season.id} label={"Сезон " + (season.seasonNumber)}>
                                                        {season.episodes.map(episode => {
                                                            return (
                                                                <option
                                                                    key={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                                    value={`${season.seasonNumber}-${episode.episodeNumber}`}
                                                                    className={currentEpisode === episode.episodeNumber && currentSeason === season.seasonNumber ?
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
                                    <select value={resolution}
                                            onChange={e => setResolution(parseInt(e.target.value))}>
                                        <option value={"360"}>360p</option>
                                        <option value={"480"}>480p</option>
                                        <option value={"720"}>720p</option>
                                        <option value={"1080"}>1080p</option>
                                    </select>
                                </div>
                            </div>
                            <ReactPlayer
                                key={videoUrl + retries}
                                url={videoUrl}
                                config={{
                                    file: {
                                        hlsOptions: {
                                            forceHLS: true,
                                            debug: false,
                                            xhrSetup: function (xhr) {
                                                xhr.setRequestHeader('Authorization', "Bearer " + sessionStorage.getItem("accessToken"));
                                            },
                                        },
                                    },
                                }}
                                controls={true}
                                height={"720px"}
                                id="videoPlayer"
                                width={"1280px"}
                            onError={updateTokenAndRetry}>
                            </ReactPlayer>
                        </div>
                    </div>
                </>
            }
        </>
    );
}
export default contentPlayer;