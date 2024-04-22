import "/src/Pages/SelectionContent/Styles/SelectionContentGenresFilter.css";
import {useState} from "react";
const SelectionContentGenresFilter = ({filter, genres}) => {
    const GenresFilter = () => {
        if(genres === undefined){
            return <label className="selection-content-info-label">Загружаем</label>
        } else if (genres === null) {
            return <label className="selection-content-info-label">Что-то пошло не так</label>
        } else{
            return genres.map((genre, index) => {
                    const [checked, setChecked] = useState(filter.genres.includes(genre.id))
                    return (
                        <div key={index}>
                            <input className="selection-content-filter-panel-genre-name-cb" type="checkbox" checked={checked} onChange={(e) => {
                                setChecked(!checked)
                                if(e.target.checked) {
                                    filter.genres.push(genre.id)
                                }else {
                                    filter.genres = filter.genres.filter((id) => id !== genre.id)
                                }
                            }}/>
                            <label className="selection-content-filter-panel-genre-name">{genre.name}</label>
                        </div>
                    )
                }
            )
        }
    }
    return (<GenresFilter/>)
}
export default SelectionContentGenresFilter;