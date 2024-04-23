import styles from './styles/contentPlayer.module.css';
import React, {useEffect, useState} from 'react';
import ReactPlayer from 'react-player';
import gif from './Images/loading-loading-forever.gif'
import {baseUrl} from "../../httpClient/baseUrl.js";
const contentPlayer = ({contentId, contentType, seasonInfos}) => {
    const [resolution, setResolution] = useState(720)
    const [error, setError] = useState(null)
    const [dataFetching, setDataFetching] = useState(false)
    const [currentEpisode, setCurrentEpisode] = useState(1)
    const [currentSeason, setCurrentSeason] = useState(1)
    const getUrl = () => {
        return baseUrl + "content/movie/video/" + contentId + "?resolution=" + resolution +
            (contentType === "сериал"? `&episode=${currentEpisode}&season=${currentSeason}` : ``)
    }
    // этот useEffect проверяет что пользователь МОЖЕТ смотреть видео(иначе у него будет окно что нельзя)
    useEffect(() => {
        async function fetchData() {
            try {
                setDataFetching(true)
                const resp = await fetch(getUrl(), {
                    headers: {
                        "Authorization": "Bearer " + sessionStorage.getItem("accessToken")
                    }
                });
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
                                        <select value={`${currentSeason}-${currentEpisode}`} onChange={(e) => {
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
                                    <select value={resolution}
                                            onChange={e => setResolution(parseInt(e.target.value))}>
                                        <option value={"360"}>360p</option>
                                        <option value={"480"}>480p</option>
                                        <option value={"720"}>720p</option>
                                        <option value={"1080"}>1080p</option>
                                    </select>
                                </div>
                            </div>
                            <CustomVideo videoUrl={getUrl()}></CustomVideo>
                        </div>
                    </div>
                </>
            }
        </>
    );
}
export default contentPlayer;

const CustomVideo = ({ videoUrl }) => {
    const options = {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("accessToken")}`
        }
    }
    const [url, setUrl] = useState()
    useEffect(() => {
        fetch(videoUrl, options)
          .then(response => response.blob())
          .then(blob => {
              setUrl(URL.createObjectURL(blob))

          });
    }, [videoUrl])


    return (
      <ReactPlayer url={url}   
                   controls 
                   height={"720px"}
                   id="videoPlayer"
                   width={"1280px"}/>
    )
}