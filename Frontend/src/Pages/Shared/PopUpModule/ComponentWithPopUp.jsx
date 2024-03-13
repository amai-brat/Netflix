import {useState} from "react";

const ComponentWithPopUp = ({Component, PopUp}) => {
    const [popUpDisplayed, setPopUpDisplayed] = useState(false)
    const handleComponentClick = () => {
        setPopUpDisplayed(!popUpDisplayed)    
    }
    return(
       <div className="pop-up-parent">
           <div onClick={handleComponentClick}>
               <Component/>
           </div>
           <div className="pop-up" style={{display: popUpDisplayed ? "block" : "none"}}>
               <PopUp/>
           </div>
       </div> 
    )
}
export default ComponentWithPopUp