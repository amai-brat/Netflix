import {useEffect, useState} from 'react'
import SelectionContentGrid from "./SelectionContentGrid.jsx";
import SelectionContentFilterPanel from "./SelectionContentFilterPanel.jsx";
import "/src/Pages/SelectionContent/Styles/SelectionContent.css";
import SelectionContentSearchPanel from "./SelectionContentSearchPanel.jsx";
import {useLocation} from "react-router-dom";
import {contentService} from "../../services/content.service.js";

const SelectionContent = () => {
    const getQueryParams = () =>
        Object.keys(filter)
            .map(key => {
                if (Array.isArray(filter[key])) {
                    if(filter[key].length === 0)
                        return '';
                    return filter[key].map(value => `${key}=${value}`).join('&');
                } else {
                    if(filter[key] === null)
                        return '';
                    return `${key}=${filter[key]}`;
                }
            })
            .filter(param => param !== '')
            .join('&');
    
    const getAllContentByFilterAsync = async () => {
        try {
            const {response, data} = await contentService.getContentsByFilter(getQueryParams());
            if(response.ok){
                setContents(data)
            }else{
                setContents(null)
            }
        }
        catch (error){
            setContents(null)
            console.error(error)
        }
    }
    
    const location = useLocation()
    const [contents, setContents] = useState(undefined)
    const [filter, setFilter] = useState({
        name: null,
        types: location?.state?.filter?.type ? [location?.state?.filter?.type] : [],
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

    useEffect(() => {
        setFilter(prevFilter => ({
            ...prevFilter,
            types: location.state?.filter.type ? [location.state?.filter?.type] : [],
        }))
    }, [location])
    
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