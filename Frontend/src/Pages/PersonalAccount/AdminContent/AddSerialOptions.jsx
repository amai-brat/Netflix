import styles from './css/AddSerialOptions.module.css'
import {useState} from "react";
const AddSerialOptions = () => {
    const [id, setId] = useState(0)
    const [name, setName] = useState("")
    const [description, setDescription] = useState("")
    const [slogan, setSlogan] = useState(null)
    const [posterUrl, setPosterUrl] = useState("")
    const [country, setCountry] = useState(null)
    const [contentType, setContentType] = useState("")
    const [ageRating, setAgeRating] = useState(null)
    const [ratings, setRatings] = useState(null)
    const [trailerInfo, setTrailerInfo] = useState(null)
    const [budget, setBudget] = useState(null)
    const [genres, setGenres] = useState([])
    const [personsInContent, setPersonsInContent] = useState([])
    const [allowedSubscriptions, setAllowedSubscriptions] = useState([])
    const [seasonInfos, setSeasonInfos] = useState([])
    const [releaseYears, setReleaseYears] = useState(null)
    return (
        <div className={styles.addSerialOptions}>
            {/*
                using this
                public class SerialContentAdminPageDto
                {
                    public long Id { get; set; }
                    public string Name { get; set; } = null!;
                    public string Description { get; set; } = null!;
                    public string? Slogan { get; set; }
                    public string PosterUrl { get; set; } = null!;
                    public string? Country { get; set; }
                    public string ContentType { get; set; } = null!;
                
                    public AgeRatings? AgeRating { get; set; }
                    public Ratings? Ratings { get; set; }
                    public TrailerInfo? TrailerInfo { get; set; }
                    public Budget? Budget { get; set; }
                
                    public List<string> Genres { get; set; } = null!;
                    public List<PersonInContentAdminPageDto> PersonsInContent { get; set; } = null!;
                    public List<SubscriptionAdminPageDto> AllowedSubscriptions { get; set; } = null!;
                    public List<SeasonInfoAdminPageDto> SeasonInfos { get; set; } = null!;
                    public YearRange ReleaseYears { get; set; } = null!;
                }                
            */}
            <input type="text" placeholder="Название" onChange={e => setName(e.target.value)}/>
            <textarea placeholder="Описание" onChange={e => setDescription(e.target.value)}/>
            <input type="text" placeholder="Слоган" onChange={e => setSlogan(e.target.value)}/>
            <input type="text" placeholder="URL постера" onChange={e => setPosterUrl(e.target.value)}/>
            <input type="text" placeholder="Страна" onChange={e => setCountry(e.target.value)}/>
            <select onChange={e => setContentType(e.target.value)} defaultValue={""}>
                <option value="" disabled={true} style={{color: "#b2aba1"}}>Тип контента</option>
                <option value="Фильм">Фильм</option>
                <option value="Сериал">Сериал</option>
            </select>

            <input type="number" placeholder="Возрастной рейтинг" onChange={e => setAgeRating(e.target.value)}/>
            <input type="text" placeholder="Рейтинг" onChange={e => setRatings(e.target.value)}/>
        </div>
    )
}
export default AddSerialOptions