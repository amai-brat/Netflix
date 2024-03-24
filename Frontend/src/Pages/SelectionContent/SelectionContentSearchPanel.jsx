import {useNavigate} from "react-router-dom";
import {useState} from "react";
import "/src/Pages/SelectionContent/Styles/SelectionContentSearchPanel.css";

const SelectionContentSearchPanel = ({filter, setFilter}) => {
    const navigate = useNavigate()
    const [searchContentName, setSearchContentName] = useState("")
    const handleChangeSearchBar = (e) => {
        setSearchContentName(e.target.value)
    }
    
    return (
        <div id="selection-content-search-panel">
            <input id="selection-content-search-panel-search-bar" type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img id="selection-content-search-panel-search-icon" src="/src/assets/Vector.svg" alt="Search" onClick={() => {

            }}/>
        </div>
    )
}
export default SelectionContentSearchPanel;