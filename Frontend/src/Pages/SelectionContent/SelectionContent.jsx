import {useEffect, useState} from 'react'
import SelectionContentGrid from "./SelectionContentGrid.jsx";
import SelectionContentFilterPanel from "./SelectionContentFilterPanel.jsx";
import "/src/Pages/SelectionContent/Styles/SelectionContent.css";
import SelectionContentSearchPanel from "./SelectionContentSearchPanel.jsx";
import {contentsData, contentTypesData} from "./TestData.jsx";
import {useLocation} from "react-router-dom";

const SelectionContent = () => {
    const getAllContentByFilterAsync = async () => {
        try {
            //TODO: Указать действительный url запроса
            const response = await fetch("https://localhost:5000/GetAllContentByFilter", {
                method: "post",
                headers:{
                    "Accept": "application/json",
                    "Content-Type": 'application/json'
                },
                body: JSON.stringify(filter)
            })
            if(response.ok){
                setContents(await response.json())
            }else{
                setContents(null)
            }
        }
        catch (error){
            //TODO: изменить на null после разработки API
            setContents(contentsData)
            console.error(error)
        }
    }
    
    const location = useLocation()
    const [contents, setContents] = useState(undefined)
    const [filter, setFilter] = useState({
        name: null,
        types: [location?.state?.filter?.type],
        genres: [],
        country: null,
        releaseYearFrom : null,
        releaseYearTo : null,
        ratingFrom : null,
        ratingTo : null
    })
    const onFilterApply = () => {
        setContents(undefined)
        getAllContentByFilterAsync()
    }
    
    useEffect(() => {
        getAllContentByFilterAsync()
    }, []);
    
    return (
        <div id="selection-content-wrapper">
            <div id="selection-content">
                <div id="selection-content-main-panel-block">
                    <SelectionContentSearchPanel 
                        filter={filter}
                        setFilter={setFilter}
                        onFilterApply={onFilterApply}
                    />
                    {contents === undefined && <label className="selection-content-main-info-label">Подождите</label>}
                    {contents === null && <label className="selection-content-main-info-label">Проблемы с подключением</label>}
                    {contents !== null && contents !== undefined &&
                        <SelectionContentGrid
                            contents={contents}
                        />
                    }
                </div>
                <div id="selection-content-filter-panel-block">
                    <SelectionContentFilterPanel
                        filter={filter}
                        setFilter={setFilter}
                        onFilterApply={onFilterApply}
                    />
                </div>
            </div>
        </div>
    )
}

export default SelectionContent