import {useEffect, useState} from 'react'
import {useLocation} from "react-router-dom";
import * as signalR from "@microsoft/signalr";
import FavouriteTabSearchPanel from "./FavouriteTabSearchPanel.jsx";
import ComponentWithPopUp from "../../Shared/PopUpModule/ComponentWithPopUp.jsx";
import ClientPanel from "../../Shared/Header/ClientPanel.jsx";
import ClientPopUpPanel from "../../Shared/Header/ClientPopUpPanel.jsx";
import FavouritesFilterButton from "./FavouritesFilterButton.jsx";
import FavouritesFilterPopUp from "./FavouritesFilterPopUp.jsx";
import FavouriteContentCard from "./FavouriteContentCard.jsx";

const FavouritesTab = () => {
    const cardPerPage = 5;
    const [favourites, setFavourites] = useState(undefined)
    const [filteredFavourites, setFilteredFavourites] = useState(undefined)
    const [currentPage, setCurrentPage] = useState(1);
    const FavouritesContent = () => {
        if(favourites === undefined){
            return <label className="favourite-info-label">Ищем</label>
        } else if(favourites === null){
            return <label className="favourite-info-label">Что-то не так, мы ничего не нашли</label>
        } else if(favourites.length === 0){
            return <label className="favourite-info-label">У вас нет избранного контента</label>
        } else{
            if(filteredFavourites.length === 0){
                return <label className="favourite-info-label">Не нашли контент с таким названием</label>
            } else{
                return filteredFavourites.slice((currentPage * cardPerPage) - cardPerPage, Math.min((currentPage * cardPerPage), filteredFavourites.length))
                    .map((content, index) => 
                    <div key={index}>
                        <FavouriteContentCard content={content.ContentBase} addedAt={content.AddedAt} userScore={content.UserScore}/>
                    </div>
                )
            }
        }
    }
    
    useEffect(() => {
        const getCurrentUserFavouritesAsync = async () => {
            try{
                //TODO: Указать действительный url запроса
                const response = await fetch("https://localhost:5000/GetAllCurrentUserFavouritesContentWithScore")
                if(response.ok){
                    const favourites = await response.json()
                    setFavourites(favourites)
                    setFilteredFavourites(favourites)
                }else{
                    setFavourites(null)
                    setFilteredFavourites(null)
                }
            }catch(error){
                var d = {}
                setFavourites(null)
                setFilteredFavourites(null)
                console.error(error)
            }
        }
        getCurrentUserFavouritesAsync()
    }, []);
    
    return (
        <div>
            <div>
                <FavouriteTabSearchPanel
                    favourites = {favourites}
                    setFavourites = {setFilteredFavourites}
                />
                <ComponentWithPopUp
                    Component = {() => <FavouritesFilterButton/>}
                    PopUp = {() => 
                        <FavouritesFilterPopUp
                            favourites = {favourites}
                            setFavourites = {setFilteredFavourites}
                        />}
                    id = {"pop-up-favourite-filter"}
                />
            </div>
            <div>
                <FavouritesContent/>
            </div>
            <div>
                { 
                    favourites !== null && 
                    favourites !== undefined && 
                    filteredFavourites.length > cardPerPage && 
                    (Array.from({ length: Math.ceil(filteredFavourites.length / cardPerPage) }, (_, index) => (
                        <button key={index} onClick={() => setCurrentPage(index + 1)}>{index + 1}</button>
                    )))
                }
            </div>
        </div>
    )
}

export default FavouritesTab