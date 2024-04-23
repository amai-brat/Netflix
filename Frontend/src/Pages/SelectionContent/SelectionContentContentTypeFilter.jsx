import "/src/Pages/SelectionContent/Styles/SelectionContentContentTypeFilter.css";
import {useState} from "react";

const SelectionContentContentTypeFilter = ({filter, contentTypes}) => {
    const ContentTypesFilter = () => {
        if(contentTypes === undefined){
            return <label className="selection-content-info-label">Загружаем</label>
        } else if (contentTypes === null) {
            return <label className="selection-content-info-label">Что-то пошло не так</label>
        } else{
            return contentTypes.map((type, index) => {
                    const [checked, setChecked] = useState(filter.types.includes(type.id))
                    return (
                        <div key={index}>
                            <input className="selection-content-filter-panel-type-name-cb" type="checkbox" checked={checked} onChange={(e) => {
                                setChecked(!checked)
                                if(e.target.checked) {
                                    filter.types.push(type.id)
                                }else {
                                    filter.types = filter.types.filter((id) => id !== type.id)
                                }
                            }}/>
                            <label className="selection-content-filter-panel-type-name">{type.contentType}</label>
                        </div>
                    )
                }
            )
        }
    }
    return (<ContentTypesFilter/>)
}
export default SelectionContentContentTypeFilter;