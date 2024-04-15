import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteContentCard.css";
import {baseUrl} from '../../Shared/HttpClient/baseUrl.js';


const FavouriteContentCard = ({content, score, addedAt}) => {
    const navigate = useNavigate()
    const [contentImg, setContentImg] = useState(content.posterUrl.toString())
    const [displayed, setDisplayed] = useState(true)

    const setDefaultContentImg = () => {
        setContentImg("/src/assets/NoImage.svg")
    }
    const navigateToViewContent = (id) => {
        navigate("/ViewContent/" + id)
    }
    
    const handleRemove = () => {
        setDisplayed(false)
        removeContentFromFavourites()
    }
    const removeContentFromFavourites = async () => {
        try{
            //TODO: Указать действительный url запроса (проверить после добавления авторизации)
            const response = await fetch(baseUrl + "content/favourite/remove", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                data: content.Id
            });
            if(!response.ok){
                setDisplayed(true)
            }
        }catch(error){
            setDisplayed(true)
        }
    }
    
    return(
        <div className="favourite-content-card" style={{display: displayed ? "flex" : "none"}}>
            <img className="favourite-content-card-poster" src={contentImg} alt="Poster" onError={setDefaultContentImg} onClick={() => {navigateToViewContent(content.id)}}/>
            <div className="favourite-content-card-data" onClick={() => {navigateToViewContent(content.id)}}>
                <label className="favourite-content-card-data-name">{content.name}</label>
                <div className="favourite-content-card-data-personal-data">
                    <label className="favourite-content-card-data-personal-data-info">Ваша оценка: {score} / 10</label>
                    <label className="favourite-content-card-data-personal-data-info">Дата добавления: {new Date(addedAt).toLocaleDateString("ru").slice(0, 10)}</label>
                </div>
            </div>
            <div className="favourite-content-card-remove-block">
                <img className="favourite-content-card-remove" src="/src/assets/Cross.svg" alt="Remove" onClick={handleRemove}/>
            </div>
        </div>
    )
}
export default FavouriteContentCard;