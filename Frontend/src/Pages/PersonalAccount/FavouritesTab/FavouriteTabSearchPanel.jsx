import {useNavigate} from "react-router-dom";
import {useState} from "react";

const FavouriteTabSearchPanel = ({favourites, setFavourites}) => {
    const [searchContentName, setSearchContentName] = useState("")
    const handleChangeSearchBar = (e) => {
        setSearchContentName(e.target.value)
    }
    const filterFavouritesByName = () => favourites.filter((content) => content.Name.contains(searchContentName))
    
    return (
        <div id="search-panel">
            <input id="search-panel-search-bar" type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img id="search-panel-search-icon" src="/src/assets/Vector.svg" alt="Search" onClick={() => {
                setFavourites(filterFavouritesByName())
            }}/>
        </div>
    )
}
export default FavouriteTabSearchPanel;