import styles from './css/EditSerialOptions.module.css'
import {useEffect, useState} from "react";
import {toast } from 'react-toastify';
import {adminSubscriptionService} from "../../../services/admin.subscription.service.js";
import {adminContentService} from "../../../services/admin.content.service.js";
const EditSerialOptions = (serialOptions) => {
    const {
        id: initialId = 0,
        name: initialName = "",
        description: initialDescription = "",
        slogan: initialSlogan = "",
        posterUrl: initialPosterUrl = "",
        bigPosterUrl: initialBigPosterUrl = "",
        country: initialCountry = "",
        contentType: initialContentType = "",
        releaseYears: {start: initialReleaseDate = "2004-03-15", end: initialEndDate = "2004-03-16"} = {},
        ageRating: initialAgeRating = null,
        ratings: initialRatings = null,
        trailerInfo: initialTrailerInfo = null,
        budget: initialBudget = null,
        genres: initialGenres = [],
        personsInContent: initialPersonsInContent = [],
        allowedSubscriptions: initialAllowedSubscriptions = [],
        allSubscriptions: initialAllSubscriptions = [],
        seasonInfos: initialSeasonInfos = [],
    } = serialOptions.serialOptions || {};
    const [id] = useState(initialId);
    const [name, setName] = useState(initialName);
    const [description, setDescription] = useState(initialDescription);
    const [slogan, setSlogan] = useState(initialSlogan);
    const [posterUrl, setPosterUrl] = useState(initialPosterUrl);
    const [bigPosterUrl, setBigPosterUrl] = useState(initialBigPosterUrl);
    const [country, setCountry] = useState(initialCountry);
    const [contentType, setContentType] = useState(initialContentType);
    const [releaseYearStart, setReleaseYearsStart] = useState(initialReleaseDate);
    const [releaseYearEnd, setReleaseYearsEnd] = useState(initialEndDate);
    const [seasonInfos, setSeasonInfos] = useState(initialSeasonInfos)
    const [ageRating, setAgeRating] = useState(initialAgeRating);
    const [ageRatingClicked, setAgeRatingClicked] = useState(ageRating !== null)
    const [ratings, setRatings] = useState(initialRatings)
    const [ratingClicked, setRatingClicked] = useState(ratings !== null && (ratings.kinopoiskRating !== null || ratings.imdbRating !== null))
    const [trailerInfo, setTrailerInfo] = useState(initialTrailerInfo)
    const [trailerInfoClicked, setTrailerInfoClicked] = useState(trailerInfo !== null)
    const [budget, setBudget] = useState(initialBudget)
    const [budgetClicked, setBudgetClicked] = useState(budget !== null)
    const [genres, setGenres] = useState(initialGenres)
    const [personsInContent, setPersonsInContent] = useState(initialPersonsInContent)
    const [allowedSubscriptions, setAllowedSubscriptions] = useState(initialAllowedSubscriptions)
    const [allSubscriptions, setAllSubscriptions] = useState(initialAllSubscriptions)
    const [personName, setPersonName] = useState("")
    const [personProfession, setPersonProfession] = useState("")
    const [seasonNumber, setSeasonNumber] = useState(0)
    const [seasonEpisodes, setSeasonEpisodes] = useState([])
    
    const addEpisode = () => {
        setSeasonEpisodes([...seasonEpisodes, {episodeNumber: 0, videoFile: null, res: 360, videoUrl: "serial/{id}/season/{season}/episode/{episode}/res/{res}/output"}])
    }
    const addSeasonAndEpisodes = () => {
        // if season already exists, then add episodes to it
        // если честно такой говнокод, да страница в целом говнокод
        const seasonIndex = seasonInfos.findIndex(s => s.seasonNumber === seasonNumber);
        if(seasonIndex !== -1) {
            const seasonInfosWithNewEpisodes = [...seasonInfos];
            seasonInfosWithNewEpisodes[seasonIndex].episodes.push(...seasonEpisodes
                .filter(item => item !== null &&
                    !seasonInfosWithNewEpisodes[seasonIndex].episodes
                        .some(e => e !== null && e.episodeNumber === item.episodeNumber)));
            setSeasonInfos(seasonInfosWithNewEpisodes);
        }
        else{
            // or if season doesn't exist, then create new season and add episodes to it
            for(let i = 0; i < seasonEpisodes.length; i++) {
                setSeasonInfos([...seasonInfos, {seasonNumber: seasonNumber,episodes: seasonEpisodes}]);
            }
        }
        setSeasonEpisodes([]);
    }
    const addPerson = () => {
        setPersonsInContent([...personsInContent, {name: personName, profession: personProfession}])
    }
    useEffect(() => {
        (async() => {
            const {response, data} = await adminSubscriptionService.getSubscriptions();
            if (response.ok) {
                setAllSubscriptions(data)
            }
            else{
                setAllSubscriptions([])
            }
        })()
    }, [])
    const handleKeyDown = (e) => {
        if (genres == null){
            setGenres([])
        }
        if (e.key === 'Enter') {
            setGenresGenres(e.target.value);
            e.preventDefault();
        }
    };
    const setGenresGenres = (value) => {
        setGenres([...genres, value])
    }
    const getNotNullGenres = () => {
        const newGenres = []
        for (let i = 0; i < genres.length; i++) {
            if (genres[i] != null) {
                newGenres.push(genres[i])
            }
        }
        return newGenres
    }
    const getNotNullPersons = () => {
        const newPersons = []
        for (let i = 0; i < personsInContent.length; i++) {
            if (personsInContent[i] != null) {
                newPersons.push(personsInContent[i])
            }
        }
        return newPersons
    }
    const getNotNullSeasonsAndEpisodes = () => {
        const newSeasons = []
        for (let i = 0; i < seasonInfos.length; i++) {
            if (seasonInfos[i] != null) {
                const newEpisodes = []
                for (let j = 0; j < seasonInfos[i].episodes.length; j++) {
                    if (seasonInfos[i].episodes[j] != null) {
                        const res = seasonInfos[i].episodes[j].res;
                        if (res === null || res === undefined) {
                            // Если res null или undefined, устанавливаем значение 360
                            newEpisodes.push({
                                ...seasonInfos[i].episodes[j],
                                res: 360
                            });
                        } else {
                            // Иначе, просто добавляем эпизод, как есть
                            newEpisodes.push(seasonInfos[i].episodes[j]);
                        }
                    }
                }
                newSeasons.push({seasonNumber: seasonInfos[i].seasonNumber, episodes: newEpisodes})
            }
        }
        return newSeasons
    }
    const Submit = async () => {
        const {response: resp, data: json} = await adminContentService.updateSerial(id, {
                id: id,
                name: name,
                description: description,
                slogan: slogan,
                posterUrl: posterUrl,
                bigPosterUrl: bigPosterUrl,
                country: country,
                contentType: contentType,
                ageRating: ageRating,
                ratings: ratings,
                trailerInfo: trailerInfo,
                budget: budget,
                genres: getNotNullGenres(),
                releaseYears: {start: releaseYearStart, end: releaseYearEnd},
                personsInContent: getNotNullPersons(),
                allowedSubscriptions: allowedSubscriptions,
                seasonInfos: getNotNullSeasonsAndEpisodes()
            });
        if (resp.ok) {
            toast.success("Сериал успешно обновлен", {
                position: "bottom-center"
            })
        } else {
            const errorMessage = json.message;
            toast.error(errorMessage , {
                position: "bottom-center"
            })
        }
    }
    const popUpTrailerInfo = () => {
        if (!trailerInfoClicked) {
            setTrailerInfo({url: "", name: ""})
        }
        else {
            setTrailerInfo(null)
        }
        setTrailerInfoClicked(!trailerInfoClicked)
    }
    const popUpBudget = () => {
        if (!budgetClicked) {
            setBudget({budgetValue: 0, budgetCurrencyName: ""})
        }
        else {
            setBudget(null)
        }
        setBudgetClicked(!budgetClicked)
    }
    const popUpAgeRating = () => {
        //hindi code here ask me for details if you don't understand
        setAgeRatingClicked(!ageRatingClicked)
        if (!ageRatingClicked) {
            setAgeRating({age: 0, ageMpaa: ""})
        }
        else {
            setAgeRating(null)
        }
        setAgeRatingClicked(!ageRatingClicked)
    }
    const setAgeRatingAge = (value) => {
        setAgeRating({...ageRating, age: value})
    }
    const setAgeRatingAgeMpaa = (value) => {
        setAgeRating({...ageRating, ageMpaa: value})
    }
    const popUpRating = () => {
        if (!ratingClicked) {
            setRatings({kinopoiskRating: 0, imdbRating: 0, localRating: 0})
        }
        else {
            setRatings(null)
        }
        setRatingClicked(!ratingClicked)
    }
    const setRatingsKinopoiskRating = (value) => {
        setRatings({...ratings, kinopoiskRating: value})
    }
    const setRatingsImdbRating = (value) => {
        setRatings({...ratings, imdbRating: value})
    }
    const setTrailerInfoUrl = (value) => {
        setTrailerInfo({...trailerInfo, url: value})
    }
    const setTrailerInfoName = (value) => {
        setTrailerInfo({...trailerInfo, name: value})
    }
    const setBudgetBudgetValue = (value) => {
        setBudget({...budget, budgetValue: value})
    }
    const setBudgetBudgetCurrencyName = (value) => {
        setBudget({...budget, budgetCurrencyName: value})
    }
    const handleRemoveGenre = index => {
        const newGenres = [...genres];
        newGenres[index] = null;
        setGenres(newGenres);
    };
    const handleRemovePerson = index => {
        const newPersons = [...personsInContent];
        newPersons[index] = null;
        setPersonsInContent(newPersons);
    }
    const handleRemoveSeason = index => {
        const newSeasons = [...seasonInfos];
        newSeasons[index] = null;
        setSeasonInfos(newSeasons);
    }
    const handleRemoveEpisode = (seasonIndex, episodeIndex) => {
        const newSeasons = [...seasonInfos];
        newSeasons[seasonIndex].episodes[episodeIndex] = null;
        setSeasonInfos(newSeasons);
    }
    return (
        <>
            <div className={styles.addSerialOptions}>
                <h2>Имя сериала</h2>
                <input type="text" value={name} placeholder="Название" onChange={e => setName(e.target.value)}></input>
                <h2>Описание</h2>
                <textarea placeholder="Описание" value={description} onChange={e => setDescription(e.target.value)}/>
                <h2>Слоган</h2>
                <input type="text" placeholder="Слоган" value={slogan} onChange={e => setSlogan(e.target.value)}/>
                <h2>Постер</h2>
                <input type="text" placeholder="URL постера" value={posterUrl} onChange={e => setPosterUrl(e.target.value)}/>
                <input type="text" placeholder="URL большого постера" value={bigPosterUrl} onChange={e => setBigPosterUrl(e.target.value)}/>
                <h2>Страна</h2>
                <input type="text" placeholder="Страна" value={country} onChange={e => setCountry(e.target.value)}/>
                <h2>Тип контента</h2>
                <select onChange={e => setContentType(e.target.value)} value={contentType}>
                    <option value="" disabled={true} style={{color: "#b2aba1"}}>Тип контента</option>
                    <option value="Фильм">Фильм</option>
                    <option value="Сериал">Сериал</option>
                </select>
                <button onClick={popUpAgeRating} style={{display:"block"}}>Указать возрастные рейтинги</button>
                {(ageRatingClicked || ageRating != null)&& <div>
                    <input type="number" value={ageRating.age} placeholder="Возрастной рейтинг" onChange={e => setAgeRatingAge(e.target.value)}/>
                    <input type="text" value={ageRating.ageMpaa} placeholder="Возрастной рейтинг MPAA"
                           onChange={e => setAgeRatingAgeMpaa(e.target.value)}/>
                </div>}
                <button onClick={popUpRating} style={{display:"block"}}>Указать рейтинги</button>
                {(ratingClicked || ratings != null) && <div>
                    <input type="number" value={ratings.kinopoiskRating} placeholder="Рейтинг Кинопоиска"
                           onChange={e => setRatingsKinopoiskRating(e.target.value)}/>
                    <input type="number" value={ratings.imdbRating} placeholder="Рейтинг IMDB" onChange={e => setRatingsImdbRating(e.target.value)}/>
                </div>}
                <button onClick={popUpBudget} style={{display:"block"}}>Указать бюджет</button>
                {(budgetClicked || budget != null) && <div>
                    <input type="number" value={budget.budgetValue} placeholder="Бюджет" onChange={e => setBudgetBudgetValue(e.target.value)}/>
                    <input type="text" value={budget.budgetCurrencyName} placeholder="Валюта бюджета"
                           onChange={e => setBudgetBudgetCurrencyName(e.target.value)}/>
                </div>}
                <button onClick={popUpTrailerInfo} style={{display:"block"}}>Указать трейлер</button>
                {(trailerInfoClicked || trailerInfo != null) && <div>
                    <input type="text" value={trailerInfo.name} placeholder="Название трейлера" onChange={e => setTrailerInfoName(e.target.value)}/>
                    <input type="text" value={trailerInfo.url} placeholder="URL трейлера" onChange={e => setTrailerInfoUrl(e.target.value)}/>
                </div>}
                <h2>Добавить жанры(enter - сохранение)</h2>
                <div>
                    {genres.map((genre, index) =>
                        genre !== null ? (
                            <div style={{ display: "block" }} key={index}>
                                <span>{genre}</span>
                                <span className={styles.trash} onClick={() => handleRemoveGenre(index)}></span>
                            </div>
                        ) : null
                    )}
                </div>
                <input type="text" placeholder="Жанр" onKeyDown={handleKeyDown}/>
                <h2>Выберите подписки</h2>
                <div className={styles.subscriptions}>
                    {allSubscriptions.map((subscription, index) =>
                        <div key={index} className={styles.subscription}>
                            <input type="checkbox"
                                   checked={allowedSubscriptions.find(s => s.name === subscription.name) != null}
                                   onChange={e => {
                                       if (e.target.checked) {
                                           setAllowedSubscriptions([...allowedSubscriptions, subscription])
                                       } else {
                                           setAllowedSubscriptions(allowedSubscriptions.filter(sub => sub.name !== subscription.name))
                                       }
                                   }}/>
                            <label>{subscription.name}</label>
                        </div>)}
                </div>
                <h2>Добавить новые персоны</h2>
                <div>
                    {personsInContent.map((person, index) =>
                        person !== null ? (
                            <div style={{ display: "block" }} key={index}>
                                <span>{person.name} - {person.profession}</span>
                                <span className={styles.trash} onClick={() => handleRemovePerson(index)}></span>
                            </div>
                        ) : null
                    )}
                </div>
                <input type="text" placeholder="Имя" onChange={e => setPersonName(e.target.value)}/>
                <input type="text" placeholder="Профессия" onChange={e => setPersonProfession(e.target.value)}/>
                <button onClick={addPerson}>Добавить</button>
                <h2 style={{display:"block"}}>Добавить сезоны</h2>
                {seasonInfos.map((s, i) =>
                {if (s !== null) return(
                    <div key={i}>
                        <h3 style={{display:"inline"}}>Сезон: {s.seasonNumber}</h3>
                        <span className={styles.trash} onClick={() => handleRemoveSeason(i)}></span>
                        {s.episodes.map((episode, j) =>
                        {if (episode !== null) return(
                            <div key={j} style={{width: "auto", display: "flex"}}>
                                
                                <p style={{display: "inline", marginRight: "1px", width: "unset"}}>Номер эпизода: <span
                                    style={{
                                        border: "1px solid white",
                                        paddingLeft: "3px",
                                        paddingRight: "3px",
                                        borderRadius: "3px"
                                    }}>{episode.episodeNumber}</span></p>
                                
                                <select className={styles.resSelect}
                                        onChange={e => {
                                            const newSeasonInfos = [...seasonInfos];
                                            newSeasonInfos[i].episodes[j].res = Number.parseInt(e.target.value);
                                            setSeasonInfos(newSeasonInfos)
                                        }}>
                                    <option value="" disabled={true} style={{color: "#b2aba1"}}>Разрешение</option>
                                    <option value="360">360</option>
                                    <option value="480">480</option>
                                    <option value="720">720</option>
                                    <option value="1080">1080</option>
                                </select>

                                <div style={{display: "inline-flex", width: "fit-content"}}>
                                    <input type={"file"} style={{paddingTop: "0px"}} onChange={e => {
                                        const newSeasonInfos = [...seasonInfos];
                                        newSeasonInfos[i].episodes[j].videoFile = e.target.files[0];
                                        setSeasonInfos(newSeasonInfos)
                                    }}></input>
                                </div>

                                <span className={styles.trash} onClick={() => handleRemoveEpisode(i, j)}></span>
                            </div>
                        )
                        }
                        )}
                    </div>
                )
                }
                )}
                <input type="number" placeholder="Номер сезона"
                       onChange={e => setSeasonNumber(Number.parseInt(e.target.value))}/>
                <h4 style={{marginTop: "0px", marginBottom: "10px"}}>Добавить серии в сезон</h4>

                {seasonEpisodes.map((episode, i) =>
                    <div key={i} style={{display: "flex"}}>
                        <input type="number" placeholder="Номер эпизода" style={{height:"max-content"}}
                               onChange={e => {
                                   if (Number.parseInt(e.target.value)) {
                                       const newEpisodes = [...seasonEpisodes];
                                       newEpisodes[i].episodeNumber = Number.parseInt(e.target.value);
                                       setSeasonEpisodes(newEpisodes);
                                   }
                               }}/>
                    </div>
                )}
                <button onClick={addEpisode} style={{display:"block"}}>+</button>
                <button onClick={addSeasonAndEpisodes} style={{display:"block"}}>Добавить сезон с эпизодами</button>
                <h2>Дата выхода</h2> <input type="date" value={releaseYearStart} onChange={e => setReleaseYearsStart(e.target.value)}/>
                <h2>Дата окончания</h2> <input type="date" value={releaseYearEnd} onChange={e => setReleaseYearsEnd(e.target.value)}/>
                <button type={"submit"} style={{backgroundColor: "red", color: "white"}} onClick={Submit}>Добавить</button>
            </div>
        </>
    )
}
export default EditSerialOptions;