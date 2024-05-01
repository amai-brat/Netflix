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
    const [retries,setRetries] = useState(0)
    const [rerender, setRerender] = useState(false)
    // это нужно в случае если токен протухнет при просмотре видео. 3 раза пытаемся получить новый, если не получается
    // то выводим ошибку 
    const updateTokenAndRetry = async () => {
        if (retries >= maxRetries){
            setOccuredError("вам нужно авторизироваться")
            return;
        }
        try{
            const {response} = await fetchAuth(getUrl(contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode),false,{},"")
            if (response.ok){
                setRetries(0)
                setOccuredError(null)
            }
            else{
                setRetries(retries + 1)
            }
        } catch (error){
            setOccuredError("произошла ошибка, блок catch:" + error.message)
            setRetries(retries + 1)
        }  finally {
            
        }
    } 
    const getUrl = (contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode) => {
        let path;
        if (contentType.contentTypeName === "Сериал") {
            path = `${baseUrl}content/serial/${contentId}/season/${currentSeason}/episode/${currentEpisode}/res/${resolution}/output.m3u8`;
        } else {
            path = `${baseUrl}content/movie/${contentId}/res/${resolution}/output.m3u8`;
        }
        return path;
    };
    // Обновляем URL каждый раз при изменении параметров
    useEffect(() => {
        setVideoUrl(getUrl(contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode));
    }, [resolution, currentSeason, currentEpisode, contentType]);
    
    // этот useEffect проверяет что пользователь МОЖЕТ смотреть видео(иначе у него будет окно что нельзя)
    // useEffect(() => {
    //     async function fetchData() {
    //         try {
    //             setDataFetching(true)
    //             const resp = await fetch(videoUrl, {
    //                 headers: {
    //                     "Authorization": "Bearer " + sessionStorage.getItem("accessToken")
    //                 }
    //             });
    //             if (!resp.ok) {
    //                 setOccuredError("Ошибка загрузки видео")
    //                 return;
    //             }
    //             setOccuredError(null)
    //         } catch (e) {
    //             setOccuredError(e.message)
    //             setDataFetching(false)
    //         } finally {
    //             setDataFetching(false)
    //         }
    //     }
    //     fetchData();
    // }, [videoUrl]);
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
                <div style={{display: occuredError == null && !dataFetching ? "block" : "none"}}>
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
                            onError={updateTokenAndRetry}
                            onProgress={() => setRetries(0)}>
                            </ReactPlayer>
                        </div>
                    </div>
                </div>
        </>
    );
}
export default contentPlayer;