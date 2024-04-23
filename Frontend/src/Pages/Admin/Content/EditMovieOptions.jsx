import styles from './css/EditMovieOptions.module.css'
import {useEffect, useState} from "react";
import { ToastContainer, toast } from 'react-toastify';
import {adminSubscriptionService} from "../../../services/admin.subscription.service.js";
import {adminContentService} from "../../../services/admin.content.service.js";
const EditMovieOptions = (movieOptions) => {
    const {
        id: initialId = 0,
        name: initialName = "",
        description: initialDescription = "",
        slogan: initialSlogan = null,
        posterUrl: initialPosterUrl = "",
        country: initialCountry = null,
        contentType: initialContentType = "",
        ReleaseDate: initialReleaseDate = "2004-03-15",
        videoUrl: initialVideoUrl = "",
        movieLength: initialMovieLength = 0,
        ageRatings: initialAgeRating = null,
        ratings: initialRatings = null,
        trailerInfo: initialTrailerInfo = null,
        budget: initialBudget = null,
        genres: initialGenres = [],
        personsInContent: initialPersonsInContent = [],
        allowedSubscriptions: initialAllowedSubscriptions = [],
        allSubscriptions: initialAllSubscriptions = []
    } = movieOptions.movieOptions || {};
    const [id, setId] = useState(initialId);
    const [name, setName] = useState(initialName);
    const [description, setDescription] = useState(initialDescription);
    const [slogan, setSlogan] = useState(initialSlogan);
    const [posterUrl, setPosterUrl] = useState(initialPosterUrl);
    const [country, setCountry] = useState(initialCountry);
    const [contentType, setContentType] = useState(initialContentType);
    const [releaseDate, setReleaseDate] = useState(initialReleaseDate);
    const [videoUrl, setVideoUrl] = useState(initialVideoUrl);
    const [movieLength, setMovieLength] = useState(initialMovieLength);

    const [ageRatings, setAgeRatings] = useState(initialAgeRating);
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

    const getNotNullPersons = () => {
        const newPersons = []
        for (let i = 0; i < personsInContent.length; i++) {
            if (personsInContent[i] != null) {
                newPersons.push(personsInContent[i])
            }
        }
        return newPersons
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
    const Submit = async () => {
        const {response: resp, data: json} = await adminContentService.updateMovie(id, {
                id: id,
                name: name,
                description: description,
                slogan: slogan,
                posterUrl: posterUrl,
                country: country,
                contentType: contentType,
                ageRatings: ageRatings,
                ratings: ratings,
                releaseDate: releaseDate,
                videoUrl: videoUrl,
                trailerInfo: trailerInfo,
                budget: budget,
                movieLength: movieLength,
                genres: getNotNullGenres(),
                personsInContent: getNotNullPersons(),
                allowedSubscriptions: allowedSubscriptions,
            });
        if (resp.ok) {
            toast.success("Фильм успешно Изменен", {
                position: "bottom-center"
            })
        } else {
            const errorMessage = json.message;
            toast.error(errorMessage , {
                position: "bottom-center"
            })
        }
    }
    const setGenresGenres = (value) => {
        setGenres([...genres, value])
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
        if (!ageRatingClicked) {
            setAgeRatings({age: 0, ageMpaa: null})
        }
        else {
            setAgeRatings(null)
        }
        setAgeRatingClicked(!ageRatingClicked)
        
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
    const setAgeRatingAge = (value) => {
        setAgeRatings({...ageRatings, age: value})
    }
    const setAgeRatingAgeMpaa = (value) => {
        setAgeRatings({...ageRatings, ageMpaa: value})
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
    return (
        <div className={styles.addSerialOptions}>
            <h2>Имя фильма</h2>
            <input
                type="text" value={name} placeholder="Название" onChange={e => setName(e.target.value)}></input>
            <h2>Описание</h2>
            <textarea placeholder="Описание" value={description} onChange={e => setDescription(e.target.value)}/>
            <h2>Слоган</h2>
            <input type="text" placeholder="Слоган" value={slogan} onChange={e => setSlogan(e.target.value)}/>
            <h2>Постер</h2>
            <input type="text" placeholder="URL постера" value={posterUrl}
                   onChange={e => setPosterUrl(e.target.value)}/>
            <h2>Страна</h2>
            <input type="text" placeholder="Страна" value={country} onChange={e => setCountry(e.target.value)}/>
            <h2>Ссылка на видео</h2>
            <input type={"text"} placeholder={"Ссылка на видео"} value={videoUrl}
                   onChange={e => setVideoUrl(e.target.value)}/>
            <h2>Длительность фильма</h2>
            <input type={"number"} placeholder={"Длительность"} value={movieLength}
                   onChange={e => setMovieLength(Number.parseInt(e.target.value))}/>
            <h2>Тип контента</h2>
            <select onChange={e => setContentType(e.target.value)} defaultValue={""} value={contentType}>
                <option value="" disabled={true} style={{color: "#b2aba1"}}>Тип контента</option>
                <option value="Фильм">Фильм</option>
                <option value="Сериал">Сериал</option>
            </select>
            <button onClick={popUpAgeRating}>Указать возрастные рейтинги</button>
            {(ageRatingClicked || ageRatings != null) && <div>
                <input type="number" value={ageRatings.age} placeholder="Возрастной рейтинг"
                       onChange={e => setAgeRatingAge(e.target.value)}/>
                <input type="text" value={ageRatings.ageMpaa} placeholder="Возрастной рейтинг MPAA"
                       onChange={e => setAgeRatingAgeMpaa(e.target.value)}/>
            </div>}
            <button onClick={popUpRating}>Указать рейтинги</button>
            {(ratingClicked || ratings != null) && <div>
                <input type="number" value={ratings.kinopoiskRating} placeholder="Рейтинг Кинопоиска"
                       onChange={e => setRatingsKinopoiskRating(e.target.value)}/>
                <input type="number" value={ratings.imdbRating} placeholder="Рейтинг IMDB"
                       onChange={e => setRatingsImdbRating(e.target.value)}/>
            </div>}
            <button onClick={popUpBudget}>Указать бюджет</button>
            {(budgetClicked || budget != null) && <div>
                <input type="number" value={budget.budgetValue} placeholder="Бюджет"
                       onChange={e => setBudgetBudgetValue(e.target.value)}/>
                <input type="text" value={budget.budgetCurrencyName} placeholder="Валюта бюджета"
                       onChange={e => setBudgetBudgetCurrencyName(e.target.value)}/>
            </div>}
            <button onClick={popUpTrailerInfo}>Указать трейлер</button>
            {(trailerInfoClicked || trailerInfo != null) && <div>
                <input type="text" value={trailerInfo.name} placeholder="Название трейлера"
                       onChange={e => setTrailerInfoName(e.target.value)}/>
                <input type="text" value={trailerInfo.url} placeholder="URL трейлера"
                       onChange={e => setTrailerInfoUrl(e.target.value)}/>
            </div>}
            <h2>Добавить жанры(enter - сохранение)</h2>
            <div style={{width:"auto"}}>
                {genres.map((genre, index) =>
                    genre !== null ? (
                        <div style={{display: "block"}} key={index}>
                            <span style={{display:"inline"}}>{genre}</span>
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
            <div style={{width:"auto"}}>
                {personsInContent.map((person, index) =>
                    person !== null ? (
                        <div style={{ display: "block" }} key={index}>
                            <span style={{display:"inline"}}>{person.name} - {person.profession}</span>
                            <span className={styles.trash} onClick={() => handleRemovePerson(index)}></span>
                        </div>
                    ) : null
                )}
            </div>
            <input type="text" placeholder="Имя" onChange={e => setPersonName(e.target.value)}/>
            <input type="text" placeholder="Профессия" onChange={e => setPersonProfession(e.target.value)}/>
            <button onClick={addPerson}>Добавить</button>

            <h2>Дата выхода</h2> <input type="date" value={releaseDate} onChange={e => setReleaseDate(e.target.value)}/>
            <button type={"submit"} style={{backgroundColor: "red", color: "white"}} onClick={Submit}>Добавить</button>
        </div>
    )
}
export default EditMovieOptions;