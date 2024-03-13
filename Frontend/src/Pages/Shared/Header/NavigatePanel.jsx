import {useNavigate} from "react-router-dom";

const NavigatePanel = () => {
    const navigate = useNavigate()
    const navigateToMainContent = () => {
        navigate("/MainContent")
    }
    const navigateToSelectionContent = (filter) => {
        navigate("/SelectionContent", {state: {filter: filter}})
    }
    
    return(
        <div id="navigate-panel">
            <img src="/src/assets/NetflixLogo.svg" alt="Netflix" onClick={navigateToMainContent}/>
            <label onClick={() => {
                navigateToSelectionContent({type: "film"})
            }}>Фильмы</label>
            <label onClick={() => {
                navigateToSelectionContent({type: "cartoon"})
            }}>Мультфильмы</label>
            <label onClick={() => {
                navigateToSelectionContent({type: "serial"})
            }}>Сериалы</label>
        </div>
    )
}
export default NavigatePanel