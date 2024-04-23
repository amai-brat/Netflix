import styles from './css/EditSerialOptions.module.css'
import {useEffect, useState} from "react";
import { ToastContainer, toast } from 'react-toastify';
const EditSerialOptions = (serialOptions) => {
    const {
        id: initialId = 0,
        name: initialName = "",
        description: initialDescription = "",
        slogan: initialSlogan = null,
        posterUrl: initialPosterUrl = "",
        country: initialCountry = null,
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
    const [id, setId] = useState(initialId);
    const [name, setName] = useState(initialName);
    const [description, setDescription] = useState(initialDescription);
    const [slogan, setSlogan] = useState(initialSlogan);
    const [posterUrl, setPosterUrl] = useState(initialPosterUrl);
    const [country, setCountry] = useState(initialCountry);
    const [contentType, setContentType] = useState(initialContentType);
    const [releaseYearStart, setReleaseYearsStart] = useState(initialReleaseDate);
    const [releaseYearEnd, setReleaseYearsEnd] = useState(initialEndDate);
    const [seasonInfos, setSeasonInfos] = useState(initialSeasonInfos)
    const [ageRating, setAgeRating] = useState(initialAgeRating);
    const [ageRatingClicked, setAgeRatingClicked] = useState(false)
    const [ratings, setRatings] = useState(initialRatings)
    const [ratingClicked, setRatingClicked] = useState(false)
    const [trailerInfo, setTrailerInfo] = useState(initialTrailerInfo)
    const [trailerInfoClicked, setTrailerInfoClicked] = useState(false)
    const [budget, setBudget] = useState(initialBudget)
    const [budgetClicked, setBudgetClicked] = useState(false)
    const [genres, setGenres] = useState(initialGenres)
    const [personsInContent, setPersonsInContent] = useState(initialPersonsInContent)
    const [allowedSubscriptions, setAllowedSubscriptions] = useState(initialAllowedSubscriptions)
    const [allSubscriptions, setAllSubscriptions] = useState(initialAllSubscriptions)
    const [personName, setPersonName] = useState("")
    const [personProfession, setPersonProfession] = useState("")
    const [seasonNumber, setSeasonNumber] = useState(0)
    const [seasonEpisodes, setSeasonEpisodes] = useState([])
    const addEpisode = () => {
        setSeasonEpisodes([...seasonEpisodes, {episodeNumber: 0, videoUrl: ""}])
    }
    const addSeasonAndEpisodes = () => {
        // if season already exists, then add episodes to it
        for (let i = 0; i < seasonInfos.length; i++) {
            if (seasonInfos[i]!= null && seasonInfos[i].seasonNumber === seasonNumber) {
                seasonInfos[i].episodes = seasonEpisodes;
                setSeasonInfos([...seasonInfos]);
                return;
            }
        }
        setSeasonInfos([...seasonInfos, {seasonNumber: seasonNumber, episodes: seasonEpisodes}]);
        setSeasonEpisodes([]);
    }
    const addPerson = () => {
        setPersonsInContent([...personsInContent, {name: personName, profession: personProfession}])
    }
    useEffect(() => {
        const resp = fetch("http://localhost:3000/subscription/getAll", {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        if (resp.ok) {
            resp.then(r => r.json()).then(r => setAllSubscriptions(r))
        }
        else{
            //only for tests TODO: remove
            setAllSubscriptions([
                {id: 1, name: "Сериалы", description: "Базовая подписка", maxResolution: 1080},
                {id: 2, name: "Фильмы", description: "Базовая подписка", maxResolution: 720}])
        }
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
    const getNotNullSeasons = () => {
        const newSeasons = []
        for (let i = 0; i < seasonInfos.length; i++) {
            if (seasonInfos[i] != null) {
                const newEpisodes = []
                for (let j = 0; j < seasonInfos[i].episodes.length; j++) {
                    if (seasonInfos[i].episodes[j] != null) {
                        newEpisodes.push(seasonInfos[i].episodes[j])
                    }
                }
                newSeasons.push({seasonNumber: seasonInfos[i].seasonNumber, episodes: newEpisodes})
            }
        }
        return newSeasons
    }
    const Submit = async () => {
        const resp = await fetch(`http://localhost:5114/content/serial/update/${id}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                id: id,
                name: name,
                description: description,
                slogan: slogan,
                posterUrl: posterUrl,
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
                seasonInfos: getNotNullSeasons()
            })
        })
        if (resp.ok) {
            toast.success("Сериал успешно обновлен", {
                position: "bottom-center"
            })
        } else {
            const json = await resp.json()
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
            setAgeRating({age: 0, ageMpaa: null})
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
                            <div key={j} style={{width: "auto"}}>
                                <p style={{display:"inline", marginRight:"1px"}}>Номер эпизода: <span style={{
                                    border: "1px solid white",
                                    paddingLeft: "3px",
                                    paddingRight: "3px",
                                    borderRadius: "3px"
                                }}>{episode.episodeNumber}</span></p>
                                <span className={styles.trash} onClick={() => handleRemoveEpisode(i, j)}></span>
                                <p>URL видео: {episode.videoUrl}</p>
                            </div>
                        )}
                        )}
                    </div>
                )}
                )}
                <input type="number" placeholder="Номер сезона"
                       onChange={e => setSeasonNumber(Number.parseInt(e.target.value))}/>
                <h4 style={{marginTop: "0px", marginBottom: "10px"}}>Добавить серии в сезон</h4>

                {seasonEpisodes.map((episode, i) =>
                    <div key={i}>
                        <input type="number" placeholder="Номер эпизода"
                               onChange={e => {
                                   const newEpisodes = [...seasonEpisodes];
                                   newEpisodes[i].episodeNumber = Number.parseInt(e.target.value);
                                   setSeasonEpisodes(newEpisodes);
                               }}/>
                        <input type="text" placeholder="URL видео" onChange={e => {
                            const newEpisodes = [...seasonEpisodes];
                            newEpisodes[i].videoUrl = e.target.value;   
                            setSeasonEpisodes(newEpisodes);
                        }}/>
                    </div>
                )}
                <button onClick={addEpisode}>+</button>
                <button onClick={addSeasonAndEpisodes} style={{display:"block"}}>Добавить сезон с эпизодами</button>
                <h2>Дата выхода</h2> <input type="date" value={releaseYearStart} onChange={e => setReleaseYearsStart(e.target.value)}/>
                <h2>Дата окончания</h2> <input type="date" value={releaseYearEnd} onChange={e => setReleaseYearsEnd(e.target.value)}/>
                <button type={"submit"} style={{backgroundColor: "red", color: "white"}} onClick={Submit}>Добавить
                </button>
            </div>
        </>
    )
}
export default EditSerialOptions;