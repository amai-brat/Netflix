import SelectionContentCard from "./SelectionContentCard.jsx";

const SelectionContentGrid = ({contents}) => {
    return(
        <div id="selection-content-grid">
            {contents.map((content) => (
                <SelectionContentCard content={content}/>
            ))}
        </div>
    )
}
export default SelectionContentGrid;