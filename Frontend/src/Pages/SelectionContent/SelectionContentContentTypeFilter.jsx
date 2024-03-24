import "/src/Pages/SelectionContent/Styles/SelectionContentContentTypeFilter.css";

const SelectionContentContentTypeFilter = ({filter, contentTypes}) => {
    const ContentTypesFilter = () => {
        if(contentTypes === undefined){
            return <label className="selection-content-info-label">Загружаем</label>
        } else if (contentTypes === null) {
            return <label className="selection-content-info-label">Что-то пошло не так</label>
        } else{
            return contentTypes.map((type, index) => {
                    const typesIds = filter.types.map((type) => type.Id)
                    return (
                        <div key={index}>
                            <input className="selection-content-filter-panel-type-name-cb" type="checkbox" checked={typesIds.includes(type.Id)} onChange={(e) => {
                                if(e.target.checked) {
                                    filter.types.append(type.Id)
                                }else {
                                    filter.types.remove(type.Id)
                                }
                            }}/>
                            <label className="selection-content-filter-panel-type-name">{type.ContentType}</label>
                        </div>
                    )
                }
            )
        }
    }
    return (<ContentTypesFilter/>)
}
export default SelectionContentContentTypeFilter;