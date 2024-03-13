import {useState} from "react";

const ClientPanel = ({user}) => {
    const [userIcon, setUserIcon] = useState(user.icon.toString())

    const setDefaultUserImg = () => {
        setUserIcon("/src/assets/DefaultUserIcon.svg")
    }

    return (
        <div id="client-panel">
            <img src="/src/assets/Notification.svg" alt="Notification"/>
            <img src="/src/assets/NotificationAlarm.svg" alt="NotificationAlarm"/>
            <img src={userIcon} alt="UserIcon" onError={setDefaultUserImg}/>
        </div>
    )
}
export default ClientPanel