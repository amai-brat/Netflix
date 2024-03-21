import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteContentCard.css";

const FavouriteContentCard = ({content, userScore, addedAt}) => {
    const navigate = useNavigate()
    const [contentImg, setContentImg] = useState(content.PosterUrl.toString())
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
            //TODO: Указать действительный url запроса
            const response = await fetch("https://localhost:5000/RemoveUserFavouritesContentById?id=" + content.Id)
            if(!response.ok){
                setDisplayed(true)
            }
        }catch(error){
            setDisplayed(true)
        }
    }
    
    return(
        <div className="favourite-content-card" style={{display: displayed ? "flex" : "none"}}>
            <img className="favourite-content-card-poster" src={contentImg} alt="Poster" onError={setDefaultContentImg} onClick={() => {navigateToViewContent(content.Id)}}/>
            <div className="favourite-content-card-data" onClick={() => {navigateToViewContent(content.Id)}}>
                <label className="favourite-content-card-data-name">{content.Name}</label>
                <div className="favourite-content-card-data-personal-data">
                    <label className="favourite-content-card-data-personal-data-info">Ваша оценка: {userScore} / 10</label>
                    <label className="favourite-content-card-data-personal-data-info">Дата добавления: {addedAt}</label>
                </div>
            </div>
            <div className="favourite-content-card-remove-block">
                <img className="favourite-content-card-remove" src="/src/assets/Cross.svg" alt="Remove" onClick={handleRemove}/>
            </div>
        </div>
    )
}
export default FavouriteContentCard;