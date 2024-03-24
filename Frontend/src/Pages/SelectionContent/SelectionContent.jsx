import { useState } from 'react'
import SelectionContentGrid from "./SelectionContentGrid.jsx";
import SelectionContentFilterPanel from "./SelectionContentFilterPanel.jsx";
import "/src/Pages/SelectionContent/Styles/SelectionContent.css";
import SelectionContentSearchPanel from "./SelectionContentSearchPanel.jsx";
import {contents} from "./TestData.jsx";


const SelectionContent = () => {
    const [filter, setFilter] =useState({
        name: null,
        types: [],
        genres: [],
        releaseYearFrom : null,
        releaseYearTo : null,
        ratingFrom : null,
        ratingTo : null
    })
    
    return (
        <div id="selection-content-wrapper">
            <div id="selection-content">
                <div id="selection-content-main-panel-block">
                    <SelectionContentSearchPanel 
                        filter={filter}
                        setFilter={setFilter}
                    />
                    <SelectionContentGrid
                        contents={contents}
                    />
                </div>
                <div id="selection-content-filter-panel-block">
                    <SelectionContentFilterPanel
                        filter={filter}
                        setFilter={setFilter}
                    />
                </div>
            </div>
        </div>
    )
}

export default SelectionContent