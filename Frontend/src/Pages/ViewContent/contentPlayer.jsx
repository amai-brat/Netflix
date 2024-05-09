import styles from './styles/contentPlayer.module.css';
import React, {useEffect, useState} from 'react';
import ReactPlayer from 'react-player';
import gif from './Images/loading-loading-forever.gif'
import {baseUrl} from "../../httpClient/baseUrl.js";
import {fetchAuth} from "../../httpClient/fetchAuth.js";
import { jwtDecode } from 'jwt-decode';
const contentPlayer = ({contentId, contentType, seasonInfos}) => {
    const [resolution, setResolution] = useState(1080)
    const [occuredError, setOccuredError] = useState(null)
    const [dataFetching, setDataFetching] = useState(false)
    const [currentEpisode, setCurrentEpisode] = useState(1)
    const [currentSeason, setCurrentSeason] = useState(1)
    const [videoUrl, setVideoUrl] = useState('');
    const updateTokenAndRetry = async () => {
        try{
            const {response} = await fetchAuth(getUrl(contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode),false,{},"")
            if (response.ok){
                setOccuredError(null)
            }
            else if(response.status === 401){
                setOccuredError("у вас нет доступа к этому контенту, попробуйте авторизироваться")
            } else if(response.status === 403) {
                setOccuredError("у вас нет доступа к этому контенту")
            }
            else if(response.status === 404){
                setOccuredError("такого контента нет")
            } else{
                setOccuredError("произошла ошибка, попробуйте позже или напишите нам")
            }
        } catch (error){
            setOccuredError("произошла ошибка:" + error.message)
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
        (async() => {
            const token = sessionStorage.getItem("accessToken");
            if (token && jwtDecode(token).exp + 10 < new Date() / 1000) {
                try {
                    const response = await fetch(`${baseUrl}auth/refresh-token`, {
                        method: "POST",
                        credentials: "include"
                    });
                    if (response.ok) sessionStorage.setItem('accessToken', await response.text());
                } catch {}
            }

            setVideoUrl(getUrl(contentId, contentType, seasonInfos, resolution, currentSeason, currentEpisode));
        })()
    }, [resolution, currentSeason, currentEpisode, contentType]);
    

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
                            onProgress={() => {setOccuredError(null)}}
                                
                            >
                            </ReactPlayer>
                        </div>
                    </div>
                </div>
        </>
    );
}
export default contentPlayer;