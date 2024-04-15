import {useEffect, useState} from 'react'
import FavouriteTabSearchPanel from "./FavouriteTabSearchPanel.jsx";
import ComponentWithPopUp from "../../Shared/PopUpModule/ComponentWithPopUp.jsx";
import FavouritesFilterButton from "./FavouritesFilterButton.jsx";
import FavouritesFilterPopUp from "./FavouritesFilterPopUp.jsx";
import FavouriteContentCard from "./FavouriteContentCard.jsx";
import "/src/Pages/PersonalAccount/FavouritesTab/Styles/FavouriteTab.css";
import {baseUrl} from '../../Shared/HttpClient/baseUrl.js';

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
                        <FavouriteContentCard content={content.contentBase} addedAt={content.addedAt} score={content.score}/>
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
                const response = await fetch(baseUrl + "user/get-favourites")
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
                        addedAt: "2020-01-15",
                        score: 10,
                        contentBase:{
                            id: 1,
                            name: "Какой-то фильм",
                            posterUrl: "/src/assets/poster_main.png"
                        }
                    },
                    {
                        addedAt: "2020-02-15",
                        score: 4,
                        contentBase:{
                            id: 2,
                            name: "фильм",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2020-01-18",
                        score: 7,
                        contentBase:{
                            id: 3,
                            name: "Абракадабра",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2020-11-15",
                        score: 9,
                        contentBase:{
                            id: 4,
                            name: "Бука",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2021-01-16",
                        score: 10,
                        contentBase:{
                            id: 5,
                            name: "Ясь",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2021-01-15",
                        score: 3,
                        contentBase:{
                            id: 6,
                            name: "Гоголь",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2023-01-01",
                        score: 2,
                        contentBase:{
                            id: 7,
                            name: "Груша",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2020-01-12",
                        score: 8,
                        contentBase:{
                            id: 8,
                            name: "Какой-то фильм",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2019-01-10",
                        score: 5,
                        contentBase:{
                            id: 9,
                            name: "Какой-то фильм",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2019-10-15",
                        score: 5,
                        contentBase:{
                            id: 10,
                            name: "Титаник",
                            posterUrl: ""
                        }
                    },
                    {
                        addedAt: "2020-07-18",
                        score: 1,
                        contentBase:{
                            id: 11,
                            name: "Какой-то фильм",
                            posterUrl: ""
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