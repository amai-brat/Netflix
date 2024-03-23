import {useNavigate} from "react-router-dom";

const SelectionContentCard = ({content}) => {
    const navigate = useNavigate()
    const navigateToViewContent = () => {
        navigate("/ViewContent/" + content.Id)
    }
    return(
        <div key={content.Id} className="selection-content-card" onClick={navigateToViewContent}>
            <img className="selection-content-card-poster" src={content.PosterUrl} alt="Poster"/>
            <label className="selection-content-card-name">{content.Name}</label>
        </div>
    )
}
export default SelectionContentCard;