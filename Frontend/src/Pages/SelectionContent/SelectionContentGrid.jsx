import SelectionContentCard from "./SelectionContentCard.jsx";
import "/src/Pages/SelectionContent/Styles/SelectionContentGrid.css";

const SelectionContentGrid = ({contents}) => {
    return(
        <div id="selection-content-grid">
            {contents.length === 0 &&
                <label className="selection-content-main-info-label">Ничего не найдено</label>}
            {contents.map((content, index) => (
                <div key={index}>
                <SelectionContentCard content={content}/>
                </div>
            ))}
        </div>
    )
}
export default SelectionContentGrid;