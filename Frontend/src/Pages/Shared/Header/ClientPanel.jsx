import {useState} from "react";

const ClientPanel = ({user}) => {
    const [userIcon, setUserIcon] = useState(!(user === null || user === undefined) ? user.icon.toString() : null)

    const setDefaultUserImg = () => {
        setUserIcon("/src/assets/DefaultUserIcon.svg")
    }

    return (
        <div id="client-panel">
            <img src={userIcon} alt="UserIcon" onError={setDefaultUserImg}/>
        </div>
    )
}
export default ClientPanel