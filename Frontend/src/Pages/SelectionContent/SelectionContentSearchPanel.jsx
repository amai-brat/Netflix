import {useNavigate} from "react-router-dom";
import {useState} from "react";
import "/src/Pages/SelectionContent/Styles/SelectionContentSearchPanel.css";

const SelectionContentSearchPanel = ({filter, setFilter, onFilterApply}) => {
    const handleChangeSearchBar = (e) => {
        filter.name = e.target.value
    }
    
    return (
        <div id="selection-content-search-panel">
            <input id="selection-content-search-panel-search-bar" type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img id="selection-content-search-panel-search-icon" src="/src/assets/Vector.svg" alt="Search" onClick={() => {
                setFilter(filter)
                onFilterApply()
            }}/>
        </div>
    )
}
export default SelectionContentSearchPanel;