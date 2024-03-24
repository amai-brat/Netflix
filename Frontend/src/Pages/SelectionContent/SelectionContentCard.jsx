import {useNavigate} from "react-router-dom";
import {useState} from "react";

const SelectionContentCard = ({content}) => {
    const navigate = useNavigate()
    const [poster, setPoster] = useState(content.PosterUrl)

    const setPosterDefault = () => {
        setPoster("/src/assets/NoImage.svg")
    }
    const navigateToViewContent = () => {
        navigate("/ViewContent/" + content.Id)
    }
    return(
        <div key={content.Id} className="selection-content-card" onClick={navigateToViewContent}>
            <img className="selection-content-card-poster" src={poster} alt="Poster" onError={setPosterDefault} style={{backgroundColor:"white"}}/>
            <label className="selection-content-card-name">{content.Name}</label>
        </div>
    )
}
export default SelectionContentCard;