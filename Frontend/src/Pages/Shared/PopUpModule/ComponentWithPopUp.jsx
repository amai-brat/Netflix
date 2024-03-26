import {useEffect, useState} from "react";
import "/src/Pages/Shared/PopUpModule/ComponentWithPopUp.css";
const ComponentWithPopUp = ({Component, PopUp, id}) => {
    const [popUpDisplayed, setPopUpDisplayed] = useState(false)

    useEffect(() => {
        const handleOutsideClick = (e) => {
            const popUpElement = document.querySelector("#"+ id)
            const componentElement = document.querySelector("#"+ id + "-com")
            if (popUpElement && !popUpElement.contains(e.target) && !componentElement.contains(e.target)) {
                setPopUpDisplayed(false);
            }
        }
        document.addEventListener("mousedown", handleOutsideClick)
    }, []);
    
    const handleComponentClick = () => {
        setPopUpDisplayed(!popUpDisplayed)    
    }
    return (
        <div className="pop-up-and-component-container">
            <div id={id + "-com"} className="component" onClick={handleComponentClick}>
                <Component/>
            </div>
            <div className="pop-up-parent">
                <div id={id} className="pop-up" style={{display: popUpDisplayed ? "block" : "none"}}>
                    <PopUp/>
                </div>
            </div> 
        </div>
    )
}
export default ComponentWithPopUp