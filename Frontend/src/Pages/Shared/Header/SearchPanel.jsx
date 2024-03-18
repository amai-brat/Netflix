import {useNavigate} from "react-router-dom";
import {useState} from "react";
import "/src/Pages/Shared/Header/Styles/SearchPanel.css";

const SearchPanel = () => {
    const navigate = useNavigate()
    const [searchContentName, setSearchContentName] = useState("")
    const handleChangeSearchBar = (e) => {
        setSearchContentName(e.target.value)
    }
    const navigateToSelectionContent = (filter) => {
        navigate("/SelectionContent", {state: {filter: filter}})
    }
    
    return (
        <div id="search-panel">
            <input id="search-panel-search-bar" type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img id="search-panel-search-icon" src="/src/assets/Vector.svg" alt="Search" onClick={() => {
                navigateToSelectionContent({name: searchContentName})
            }}/>
        </div>
    )
}
export default SearchPanel