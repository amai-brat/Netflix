import {useEffect, useState} from "react";
import SelectionContentGenresFilter from "/SelectionContentGenresFilter.jsx";
import SelectionContentContentTypeFilter from "./SelectionContentContentTypeFilter.jsx";

const SelectionContentFilterPanel = ({filter, setFilter}) => {
    const [selectedGenres, setSelectedGenres] = useState([])
    const [genresDisplayed, setGenresDisplayed] = useState(false)
    const [contentTypes, setContentTypes] = useState(undefined)
    const [genres, setGenres] = useState(undefined)

    useEffect(() => {
        const getAllContentTypesAsync = async () => {
            try {
                //TODO: Указать действительный url запроса
                const response = await fetch("https://localhost:5000/GetAllContentTypes")
                if(response.ok){
                    setContentTypes(await response.json())
                }else{
                    setContentTypes(null)
                }
            }
            catch (error){
                setContentTypes(null)
                console.error(error)
            }
        }
        const getAllGenresAsync = async () => {
            try{
                //TODO: Указать действительный url запроса
                const response = await fetch("https://localhost:5000/GetAllGenres")
                if(response.ok){
                    setGenres(await response.json())
                }else{
                    setContentTypes(null)
                }
            }
            catch (error){
                setContentTypes(null)
                console.error(error)
            }
        }
        getAllContentTypesAsync()
        getAllGenresAsync()
    }, []);
    
    return(
        <div id="selection-content-filter-panel">
            <div id="selection-content-filter-panel-genres-only" style={{display: genresDisplayed ? "flex": "none"}}>
                <label className="selection-content-filter-panel-genres-only-back" onClick={() => {setGenresDisplayed(false)}}>Назад</label>
                <SelectionContentGenresFilter
                    filter={filter}
                    genres={genres}
                />
            </div>
            <div id="selection-content-filter-panel-all" style={{display: genresDisplayed ? "none" : "flex"}}>
                <div id="selection-content-filter-panel-genres">
                    <label className="selection-content-filter-panel-filter-name">Жанры</label>
                    <label className="selection-content-filter-panel-genres-names" onClick={() => {setGenresDisplayed(true)}}>
                        {selectedGenres.length <= 0 && <>"Любые"</>}
                        {selectedGenres.length >= 1 && 
                            <>{
                            selectedGenres
                                .slice(0, Math.min(selectedGenres.length, 2))
                                .join(", ")
                            }{selectedGenres.length >= 3 && <>...</>}</>
                        }
                    </label>
                </div>
                <div id="selection-content-filter-panel-type">
                    <label className="selection-content-filter-panel-filter-name">Тип</label>
                    <SelectionContentContentTypeFilter
                        filter={filter}
                        contentTypes={contentTypes}
                    />
                </div>
                <div id="selection-content-filter-panel-country">
                    <label htmlFor="selection-content-filter-panel-country-select" className="selection-content-filter-panel-filter-name">Страна</label>
                    <select id="selection-content-filter-panel-country-select">
                        <!-- Не нашёл в domain страну производства-->
                        <option value={1}>США</option>
                        <option value={2}>Россия</option>
                        <option value={3}>Китай</option>
                    </select>
                </div>
                <div id="selection-content-filter-panel-year">
                    <label className="selection-content-filter-panel-filter-name">Год выпуска</label>
                    <input className="selection-content-filter-panel-filter-input" type="number" onChange={(e) => {
                        if (e.target.value === null || e.target.value === undefined || e.target.value === "") {
                            filter.releaseYearFrom = null
                        } else {
                            filter.releaseYearFrom = parseInt(e.target.value)
                        }
                    }}/>
                    <input className="selection-content-filter-panel-filter-input" type="number" onChange={(e) => {
                        if (e.target.value === null || e.target.value === undefined || e.target.value === "") {
                            filter.releaseYearTo = null
                        } else {
                            filter.releaseYearTo = parseInt(e.target.value)
                        }
                    }}/>
                </div>
                <div id="selection-content-filter-panel-rating">
                    <label className="selection-content-filter-panel-filter-name">Рейтинг</label>
                    <input className="selection-content-filter-panel-filter-input" type="number" onChange={(e) => {
                        if (e.target.value === null || e.target.value === undefined || e.target.value === "") {
                            filter.ratingFrom = null
                        } else {
                            filter.ratingFrom = parseInt(e.target.value)
                        }
                    }}/>
                    <input className="selection-content-filter-panel-filter-input" type="number" onChange={(e) => {
                        if (e.target.value === null || e.target.value === undefined || e.target.value === "") {
                            filter.ratingTo = null
                        } else {
                            filter.ratingTo = parseInt(e.target.value)
                        }
                    }}/>
                </div>
                <div id="selection-content-filter-panel-apply-and-reset">
                    <input id="selection-content-filter-panel-apply-button" type="button" value="Применить"
                           onClick={() => {
                               setFilter(filter)
                           }}/>
                    <input id="selection-content-filter-panel-reset-button" type="button" value="Сбросить"
                           onClick={() => {
                               setFilter({
                                   types: [],
                                   genres: [],
                                   releaseYearFrom: null,
                                   releaseYearTo: null,
                                   ratingFrom: null,
                                   ratingTo: null
                               })
                           }}/>
                </div>
            </div>
        </div>
    )
}

export default SelectionContentFilterPanel;