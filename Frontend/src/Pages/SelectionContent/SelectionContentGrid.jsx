import SelectionContentCard from "./SelectionContentCard.jsx";
import "/src/Pages/SelectionContent/Styles/SelectionContentGrid.css";

const SelectionContentGrid = ({contents}) => {
    return(
        <div id="selection-content-grid">
            {contents.map((content, index) => (
                <div key={index}>
                    <SelectionContentCard content={content}/>
                </div>
            ))}
        </div>
    )
}
export default SelectionContentGrid;