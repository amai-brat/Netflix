import {useNavigate} from "react-router-dom";
import "/src/Pages/Shared/Header/Styles/NavigatePanel.css";
import logo from "../../../assets/logo.png";
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
            <img id="navigate-panel-logo" className="navigate-panel-element" src={logo} alt="Voltorka" onClick={navigateToMainContent}/>
            <label className="navigate-panel-element" onClick={() => {
                navigateToSelectionContent({type: -1}) //TODO изменить на тип, который из базы данных
            }}>Фильмы</label>
            <label className="navigate-panel-element" onClick={() => {
                navigateToSelectionContent({type: -3}) //TODO изменить на тип, который из базы данных
            }}>Мультфильмы</label>
            <label className="navigate-panel-element" onClick={() => {
                navigateToSelectionContent({type: -2}) //TODO изменить на тип, который из базы данных
            }}>Сериалы</label>
        </div>
    )
}
export default NavigatePanel