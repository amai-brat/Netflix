import styles from './css/AddMovieOptions.module.css'
import {useEffect, useState} from "react";
import { ToastContainer, toast } from 'react-toastify';

const AddMovieOptions = () => {
    const [id, setId] = useState(0)
    const [name, setName] = useState("")
    const [description, setDescription] = useState("")
    const [slogan, setSlogan] = useState(null)
    const [posterUrl, setPosterUrl] = useState("")
    const [country, setCountry] = useState(null)
    const [contentType, setContentType] = useState("")

    // логика других полей в том что они имеют неправильные начальные значения
    // и при отправке на сервере проблем с биндингом нет, но есть с валидатором - он отклоняет 
    // но с Date нельзя создать намеренно неправильное значение, биндер asp.net core выбрасывает свое исключение
    // до самого валидатора. поэтому будем считать что у всех фильмов есть дата по умолчанию( мое день рождение )
    const [releaseDate,setReleaseDate] = useState("2004-03-15")
    const [videoUrl,setVideoUrl] = useState("")
    const [movieLength, setMovieLength] = useState(0)
    
    const [ageRating, setAgeRating] = useState(null)
    const [ageRatingClicked, setAgeRatingClicked] = useState(false)
    const [ratings, setRatings] = useState(null)
    const [ratingClicked, setRatingClicked] = useState(false)
    const [trailerInfo, setTrailerInfo] = useState(null)
    const [trailerInfoClicked, setTrailerInfoClicked] = useState(false)
    const [budget, setBudget] = useState(null)
    const [budgetClicked, setBudgetClicked] = useState(false)
    const [genres, setGenres] = useState([])
    const [personsInContent, setPersonsInContent] = useState([])
    const [allowedSubscriptions, setAllowedSubscriptions] = useState([])
    const [allSubscriptions, setAllSubscriptions] = useState([])
    const [personName, setPersonName] = useState("")
    const [personProfession, setPersonProfession] = useState("")

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
    const Submit = async () => {
        const resp = await fetch("http://localhost:5114/content/movie/add", {
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
                ageRatings: ageRating,
                ratings: ratings,
                releaseDate: releaseDate,
                videoUrl: videoUrl,
                trailerInfo: trailerInfo,
                budget: budget,
                movieLength: movieLength,
                genres: genres,
                personsInContent: personsInContent,
                allowedSubscriptions: allowedSubscriptions,
            })
        })
        if (resp.ok) {
            toast.success("Фильм успешно добавлен", {
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
    const setGenresGenres = (value) => {
        setGenres([...genres, value])
    }
    const popUpTrailerInfo = () => {
        setTrailerInfoClicked(!trailerInfoClicked)
        if (trailerInfoClicked) {
            setTrailerInfo({url: "", name: ""})
        }
        else {
            setTrailerInfo(null)
        }
    }
    const popUpBudget = () => {
        setBudgetClicked(!budgetClicked)
        if (budgetClicked) {
            setBudget({budgetValue: 0, budgetCurrencyName: ""})
        }
        else {
            setBudget(null)
        }
    }
    const popUpAgeRating = () => {
        setAgeRatingClicked(!ageRatingClicked)
        if (ageRatingClicked) {
            setAgeRating({age: 0, ageMpaa: null})
        }
        else {
            setAgeRating(null)
        }
    }
    const popUpRating = () => {
        setRatingClicked(!ratingClicked)
        if (ratingClicked) {
            setRatings({kinopoiskRating: 0, imdbRating: 0, localRating: 0})
        }
        else {
            setRatings(null)
        }
    }
    const setAgeRatingAge = (value) => {
        setAgeRating({...ageRating, age: value})
    }
    const setAgeRatingAgeMpaa = (value) => {
        setAgeRating({...ageRating, ageMpaa: value})
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
    return (
        <div className={styles.addSerialOptions}>
            <h2>Добавление сериала</h2>
            <input type="text" placeholder="Название" onChange={e => setName(e.target.value)}/>
            <h2>Описание</h2>
            <textarea placeholder="Описание" onChange={e => setDescription(e.target.value)}/>
            <h2>Слоган</h2>
            <input type="text" placeholder="Слоган" onChange={e => setSlogan(e.target.value)}/>
            <h2>Постер</h2>
            <input type="text" placeholder="URL постера" onChange={e => setPosterUrl(e.target.value)}/>
            <h2>Страна</h2>
            <input type="text" placeholder="Страна" onChange={e => setCountry(e.target.value)}/>
            <h2>Ссылка на видео</h2>
            <input type={"text"} placeholder={"Ссылка на видео"} onChange={e => setVideoUrl(e.target.value)}/>
            <h2>Длительность фильма</h2>
            <input type={"number"} placeholder={"Длительность"} onChange={e => setMovieLength(Number.parseInt(e.target.value))}/>
            <h2>Тип контента</h2>
            <select onChange={e => setContentType(e.target.value)} defaultValue={""}>
                <option value="" disabled={true} style={{color: "#b2aba1"}}>Тип контента</option>
                <option value="Фильм">Фильм</option>
                <option value="Сериал">Сериал</option>
            </select>
            <button onClick={popUpAgeRating}>Указать возрастные рейтинги</button>
            {ageRatingClicked && <div>
                <input type="number" placeholder="Возрастной рейтинг" onChange={e => setAgeRatingAge(e.target.value)}/>
                <input type="text" placeholder="Возрастной рейтинг MPAA"
                       onChange={e => setAgeRatingAgeMpaa(e.target.value)}/>
            </div>}
            <button onClick={popUpRating}>Указать рейтинги</button>
            {ratingClicked && <div>
                <input type="number" placeholder="Рейтинг Кинопоиска"
                       onChange={e => setRatingsKinopoiskRating(e.target.value)}/>
                <input type="number" placeholder="Рейтинг IMDB" onChange={e => setRatingsImdbRating(e.target.value)}/>
            </div>}
            <button onClick={popUpBudget}>Указать бюджет</button>
            {budgetClicked && <div>
                <input type="number" placeholder="Бюджет" onChange={e => setBudgetBudgetValue(e.target.value)}/>
                <input type="text" placeholder="Валюта бюджета"
                       onChange={e => setBudgetBudgetCurrencyName(e.target.value)}/>
            </div>}
            <button onClick={popUpTrailerInfo}>Указать трейлер</button>
            {trailerInfoClicked && <div>
                <input type="text" placeholder="Название трейлера" onChange={e => setTrailerInfoName(e.target.value)}/>
                <input type="text" placeholder="URL трейлера" onChange={e => setTrailerInfoUrl(e.target.value)}/>
            </div>}
            <h2>Добавить жанры(enter - сохранение)</h2>
            {genres !== null &&
                <div className={styles.existingGenres}>
                    {genres.map((genre, index) => <div key={index}>{genre.toString()}</div>)}
                </div>
            }
            <input type="text" placeholder="Жанр" onKeyDown={handleKeyDown}/>
            <h2>Выберите подписки</h2>
            <div className={styles.subscriptions}>
                {allSubscriptions.map((subscription, index) =>
                    <div key={index} className={styles.subscription}>
                        <input type="checkbox" onChange={e => {
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
            {personsInContent.map((p, i) =>
                <div key={i}>{p.name} - {p.profession}</div>)
            }
            <input type="text" placeholder="Имя" onChange={e => setPersonName(e.target.value)}/>
            <input type="text" placeholder="Профессия" onChange={e => setPersonProfession(e.target.value)}/>
            <button onClick={addPerson}>Добавить</button>
            
            <h2>Дата выхода</h2> <input type="date" onChange={e => setReleaseDate(e.target.value)}/>
            <button type={"submit"} style={{backgroundColor: "red", color: "white"}} onClick={Submit}>Добавить</button>
        </div>
    )
}
export default AddMovieOptions