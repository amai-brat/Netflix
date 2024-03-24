const SelectionContentGenresFilter = ({filter, contentTypes}) => {
    const ContentTypesFilter = () => {
        if(contentTypes === undefined){
            return <label>Загружаем</label>
        } else if (contentTypes === null) {
            return <label>Что-то пошло не так</label>
        } else{
            return contentTypes.map((type, index) => {
                    const typesIds = filter.types.map((type) => type.Id)
                    return (
                        <div key={index}>
                            <input type="checkbox" checked={typesIds.includes(type.Id)} onChange={(e) => {
                                if(e.target.checked) {
                                    filter.types.append(type.Id)
                                }else {
                                    filter.types.remove(type.Id)
                                }
                            }}/>
                        </div>
                    )
                }
            )
        }
    }
    return (<ContentTypesFilter/>)
}
export default SelectionContentGenresFilter;