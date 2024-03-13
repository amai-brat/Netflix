import {useNavigate} from "react-router-dom";
import {useState} from "react";

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
            <input type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img src="/src/assets/Vector.svg" alt="Search" onClick={() => {
                navigateToSelectionContent({name: searchContentName})
            }}/>
        </div>
    )
}
export default SearchPanel