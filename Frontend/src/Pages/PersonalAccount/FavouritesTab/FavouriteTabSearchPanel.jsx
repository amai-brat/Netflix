import {useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteTabSearchPanel.css";
import vector from "../../../assets/Vector.svg";
const FavouriteTabSearchPanel = ({favourites, setFavourites}) => {
    const [searchContentName, setSearchContentName] = useState("")
    const handleChangeSearchBar = (e) => {
        setSearchContentName(e.target.value)
    }
    const filterFavouritesByName = () => favourites.filter((content) => 
        content.contentBase.name.toLowerCase().indexOf(searchContentName.toLowerCase()) >= 0)
    
    return (
        <div id="favourite-search-panel">
            <input id="favourite-search-panel-search-bar" type="text" placeholder="Поиск по названию" onChange={handleChangeSearchBar}/>
            <img id="favourite-search-panel-search-icon" src={vector} alt="Search" onClick={() => {
                setFavourites(filterFavouritesByName())
            }}/>
        </div>
    )
}
export default FavouriteTabSearchPanel;