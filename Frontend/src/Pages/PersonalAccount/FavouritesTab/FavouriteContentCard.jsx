import {useLocation, useNavigate} from "react-router-dom";

const FavouriteContentCard = ({content, userScore, addedAt}) => {
    const navigate = useNavigate()
    const navigateToViewContent = (id) => {
        navigate("/ViewContent" + id)
    }
    
    return(
        <div className="favourite-content-card">
            <img className="favourite-content-card-poster" src={content.posterUrl.toString()} alt="Poster"/>
            <div className="favourite-content-card-data" onClick={() => {navigateToViewContent(content.id)}}>
                <label className="favourite-content-card-data-name">{content.contentName}</label>
                <div className="favourite-content-card-data-personal-data">
                    <label className="favourite-content-card-data-personal-data-info">{userScore}</label>
                    <label className="favourite-content-card-data-personal-data-info">{addedAt}</label>
                </div>
            </div>
            <img className="favourite-content-card-remove" src="/src/assets/Cross.svg" alt="Remove"/>
        </div>
    )
}
export default FavouriteContentCard;