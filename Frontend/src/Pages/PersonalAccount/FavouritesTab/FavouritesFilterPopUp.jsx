import {useState} from "react";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouritesFilterPopUp.css";

const FavouritesFilterPopUp = ({favourites, setFavourites}) => {
    const filterFavouritesByRule = (rule) => {
        switch (rule){
            case "score":
                return favourites.slice().sort((a,b) => b.score - a.score);
            case "date":
                return favourites.slice().sort((a,b) => new Date(b.addedAt) - new Date(a.addedAt))
            case "name":
                return favourites.slice().sort((a,b) => a.contentBase.name.localeCompare(b.contentBase.name))
            default:
                return favourites.slice()
        }
    }
    
    return(
        <div id="favourites-filter-pop-up">
            <div className="favourites-filter-pop-up-filter">
                <label className="favourites-filter-pop-up-filter-rule" onClick={() => {
                    setFavourites(filterFavouritesByRule("name"))
                }}>По названию</label>
            </div>
            <div className="favourites-filter-pop-up-filter">
                <label className="favourites-filter-pop-up-filter-rule" onClick={() => {
                    setFavourites(filterFavouritesByRule("score"))
                }}>По оценке</label>
            </div>
            <div className="favourites-filter-pop-up-filter">
                <label className="favourites-filter-pop-up-filter-rule" onClick={() => {
                    setFavourites(filterFavouritesByRule("date"))
                }}>По дате</label>
            </div>
        </div>
    )
}
export default FavouritesFilterPopUp;