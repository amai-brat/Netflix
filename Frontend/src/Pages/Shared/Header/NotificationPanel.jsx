import {useEffect, useState} from "react";

const NotificationPanel = ({alarmed}) => {
    return(
        <div id="notification-panel">
            <img src="/src/assets/Notification.svg" alt="Notification" style={{display: alarmed ? "none" : "block"}}/>
            <img src="/src/assets/NotificationAlarm.svg" alt="NotificationAlarm" style={{display: alarmed ? "block" : "none"}}/>
        </div>
    )
}
export default NotificationPanel