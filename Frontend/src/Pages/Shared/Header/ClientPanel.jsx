import {useState} from "react";
import "/src/Pages/Shared/Header/Styles/ClientPanel.css";
import defaultIcon from "../../../assets/default.png"
const ClientPanel = ({user}) => {
    const [userIcon, setUserIcon] = useState(user && user.icon ? user.icon : defaultIcon)

    const setDefaultUserImg = () => {
        setUserIcon(defaultIcon)
    }

    return (
        <div id="client-panel">
            <img id="client-panel-person-icon" src={userIcon} alt="UserIcon" onError={setDefaultUserImg}/>
        </div>
    )
}
export default ClientPanel