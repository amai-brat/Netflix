import "/src/Pages/SelectionContent/Styles/SelectionContentGenresFilter.css";
const SelectionContentGenresFilter = ({filter, genres}) => {
    const GenresFilter = () => {
        if(genres === undefined){
            return <label className="selection-content-info-label">Загружаем</label>
        } else if (genres === null) {
            return <label className="selection-content-info-label">Что-то пошло не так</label>
        } else{
            return genres.map((genre, index) => {
                    const genresIds = filter.genres.map((genre) => genre.Id)
                    return (
                        <div key={index}>
                            <input className="selection-content-filter-panel-genre-name-cb" type="checkbox" checked={genresIds.includes(genre.Id)} onChange={(e) => {
                                if(e.target.checked) {
                                    filter.genres.append(genre.Id)
                                }else {
                                    filter.genres.remove(genre.Id)
                                }
                            }}/>
                            <label className="selection-content-filter-panel-genre-name">{genre.Name}</label>
                        </div>
                    )
                }
            )
        }
    }
    return (<GenresFilter/>)
}
export default SelectionContentGenresFilter;