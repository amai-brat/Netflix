import {useState} from "react";
import "/src/Pages/Shared/Header/Styles/ClientPanel.css";
const ClientPanel = ({user}) => {
    const [userIcon, setUserIcon] = useState(!(user === null || user === undefined) ? user.icon.toString() : null)

    const setDefaultUserImg = () => {
        setUserIcon("/src/assets/DefaultUserIcon.svg")
    }

    return (
        <div id="client-panel">
            <img id="client-panel-person-icon" src={userIcon} alt="UserIcon" onError={setDefaultUserImg}/>
        </div>
    )
}
export default ClientPanel