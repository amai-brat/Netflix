import {useEffect, useState} from 'react'
import FavouriteTabSearchPanel from "./FavouriteTabSearchPanel.jsx";
import ComponentWithPopUp from "../../Shared/PopUpModule/ComponentWithPopUp.jsx";
import FavouritesFilterButton from "./FavouritesFilterButton.jsx";
import FavouritesFilterPopUp from "./FavouritesFilterPopUp.jsx";
import FavouriteContentCard from "./FavouriteContentCard.jsx";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteTab.css";

const FavouritesTab = () => {
    const cardPerPage = 5;
    const [favourites, setFavourites] = useState(undefined)
    const [filteredFavourites, setFilteredFavourites] = useState(undefined)
    const [currentPage, setCurrentPage] = useState(1);
    const FavouritesContent = () => {
        if(favourites === undefined){
            return <label className="favourite-info-label">Ищем . . .</label>
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
    
    const resetFavouriteContent = ()=> {
        setFilteredFavourites(favourites)
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
                const testData = [
                    {
                        AddedAt: "2020-01-15",
                        UserScore: 10,
                        ContentBase:{
                            Id: 1,
                            Name: "Какой-то фильм",
                            PosterUrl: "/src/assets/poster_main.png"
                        }
                    },
                    {
                        AddedAt: "2020-02-15",
                        UserScore: 4,
                        ContentBase:{
                            Id: 2,
                            Name: "фильм",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2020-01-18",
                        UserScore: 7,
                        ContentBase:{
                            Id: 3,
                            Name: "Абракадабра",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2020-11-15",
                        UserScore: 9,
                        ContentBase:{
                            Id: 4,
                            Name: "Бука",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2021-01-16",
                        UserScore: 10,
                        ContentBase:{
                            Id: 5,
                            Name: "Ясь",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2021-01-15",
                        UserScore: 3,
                        ContentBase:{
                            Id: 6,
                            Name: "Гоголь",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2023-01-01",
                        UserScore: 2,
                        ContentBase:{
                            Id: 7,
                            Name: "Груша",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2020-01-12",
                        UserScore: 8,
                        ContentBase:{
                            Id: 8,
                            Name: "Какой-то фильм",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2019-01-10",
                        UserScore: 5,
                        ContentBase:{
                            Id: 9,
                            Name: "Какой-то фильм",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2019-10-15",
                        UserScore: 5,
                        ContentBase:{
                            Id: 10,
                            Name: "Титаник",
                            PosterUrl: ""
                        }
                    },
                    {
                        AddedAt: "2020-07-18",
                        UserScore: 1,
                        ContentBase:{
                            Id: 11,
                            Name: "Какой-то фильм",
                            PosterUrl: ""
                        }
                    },
                ]
                setFavourites(testData)
                setFilteredFavourites(testData)
                console.error(error)
            }
        }
        getCurrentUserFavouritesAsync()
    }, []);
    
    return (
        <div id="favourite-tab">
            <div id="favourite-search-and-filter-panel">
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
                <img id="favourite-reset-filter" src="/src/assets/ResetFilter.svg" alt="Reset" onClick={resetFavouriteContent}/>
            </div>
            <div id="favourites-content">
                <FavouritesContent/>
            </div>
            <div id="favourite-navigate-panel">
                { 
                    favourites !== null && 
                    favourites !== undefined && 
                    filteredFavourites.length > cardPerPage && 
                    (Array.from({ length: Math.ceil(filteredFavourites.length / cardPerPage) }, (_, index) => (
                        <label key={index} className="favourite-navigation-digit" onClick={() => setCurrentPage(index + 1)}>{index + 1}</label>
                    )))
                }
            </div>
        </div>
    )
}

export default FavouritesTab