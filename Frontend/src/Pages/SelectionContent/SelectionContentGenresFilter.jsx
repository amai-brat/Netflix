const SelectionContentGenresFilter = ({filter, genres}) => {
    const GenresFilter = () => {
        if(genres === undefined){
            return <label>Загружаем</label>
        } else if (genres === null) {
            return <label>Что-то пошло не так</label>
        } else{
            return genres.map((genre, index) => {
                    const genresIds = filter.genres.map((genre) => genre.Id)
                    return (
                        <div key={index}>
                            <input type="checkbox" checked={genresIds.includes(genre.Id)} onChange={(e) => {
                                if(e.target.checked) {
                                    filter.genres.append(genre.Id)
                                }else {
                                    filter.genres.remove(genre.Id)
                                }
                            }}/>
                        </div>
                    )
                }
            )
        }
    }
    return (<GenresFilter/>)
}
export default SelectionContentGenresFilter;