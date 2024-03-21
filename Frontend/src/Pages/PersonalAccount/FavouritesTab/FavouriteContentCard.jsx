import {useNavigate} from "react-router-dom";
import {useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteContentCard.css";

const FavouriteContentCard = ({content, userScore, addedAt}) => {
    const navigate = useNavigate()
    const [contentImg, setContentImg] = useState(content.PosterUrl.toString())

    const setDefaultContentImg = () => {
        setContentImg("/src/assets/NoImage.svg")
    }
    const navigateToViewContent = (id) => {
        navigate("/ViewContent/" + id)
    }
    
    return(
        <div className="favourite-content-card">
            <img className="favourite-content-card-poster" src={contentImg} alt="Poster" onError={setDefaultContentImg} onClick={() => {navigateToViewContent(content.Id)}}/>
            <div className="favourite-content-card-data" onClick={() => {navigateToViewContent(content.Id)}}>
                <label className="favourite-content-card-data-name">{content.Name}</label>
                <div className="favourite-content-card-data-personal-data">
                    <label className="favourite-content-card-data-personal-data-info">Ваша оценка: {userScore} / 10</label>
                    <label className="favourite-content-card-data-personal-data-info">Дата добавления: {addedAt}</label>
                </div>
            </div>
            <div className="favourite-content-card-remove-block">
                <img className="favourite-content-card-remove" src="/src/assets/Cross.svg" alt="Remove"/>
            </div>
        </div>
    )
}
export default FavouriteContentCard;