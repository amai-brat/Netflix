import {useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouritesFilterPopUp.css";

const FavouritesFilterPopUp = ({favourites, setFavourites}) => {
    const [filterName, setFilterName] = useState(false)
    const [filterDate, setFilterDate] = useState(false)
    const [filterScore, setFilterScore] = useState(false)
    const filters = [
        {rule:"score", set: setFilterScore},
        {rule:"date", set: setFilterDate},
        {rule:"name", set: setFilterName}
    ]
    const filterFavouritesByRule = (rule) => {
        setCheckboxByRule(rule)
        switch (rule){
            case "score":
                return favourites.sort((a,b) => a.Score - b.Score)
            case "date":
                return favourites.sort((a,b) => a.AddedAt - b.AddedAt)
            case "name":
                return favourites.sort((a,b) => a.Name.localeCompare(b.Name))
            default:
                return favourites
        }
    }
    const setCheckboxByRule = (rule) => {
        filters.forEach((filter) => {
            if(filter.rule !== rule){
                filter.set(false)
            }else{
                filter.set(true)
            }
        }) 
    }
    
    return(
        <div id="favourites-filter-pop-up">
            <div className="favourites-filter-pop-up-filter">
                <input id="favourites-filter-pop-up-filter-name" type="checkbox" checked={filterName} onChange={() => {
                    setFavourites(filterFavouritesByRule("name"))
                }}/>
                <label htmlFor="favourites-filter-pop-up-filter-name">По названию</label>
            </div>
            <div className="favourites-filter-pop-up-filter">
                <input id="favourites-filter-pop-up-filter-score" type="checkbox" checked={filterScore}
                       onChange={() => {
                           setFavourites(filterFavouritesByRule("score"))
                       }}/>
                <label htmlFor="favourites-filter-pop-up-filter-score">По оценке</label>
            </div>
            <div className="favourites-filter-pop-up-filter">
                <input id="favourites-filter-pop-up-filter-date" type="checkbox" checked={filterDate} onChange={() => {
                    setFavourites(filterFavouritesByRule("date"))
                }}/>
                <label htmlFor="favourites-filter-pop-up-filter-date">По дате</label>
            </div>
        </div>
    )
}
export default FavouritesFilterPopUp;