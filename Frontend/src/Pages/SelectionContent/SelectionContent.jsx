import { useState } from 'react'
import SelectionContentGrid from "./SelectionContentGrid.jsx";
import SelectionContentFilterPanel from "./SelectionContentFilterPanel.jsx";



const SelectionContent = () => {
    const [filter, setFilter] =useState({
        name: null,
        type: null,
        genres: [],
        releaseYearFrom : null,
        releaseYearTo : null,
        ratingFrom : null,
        ratingTo : null
    })
    
    return (
        <div>
            <div>
                <SelectionContentGrid
                    contents={[{Id: 1, Name: "asd", PosterUrl: "dsd"}]}
                />
            </div>
            <div>
                <SelectionContentFilterPanel
                    filter={filter}
                    setFilter={setFilter}
                />
            </div>
        </div>
    )
}

export default SelectionContent