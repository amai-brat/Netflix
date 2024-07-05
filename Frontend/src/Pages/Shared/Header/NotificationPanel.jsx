import "/src/Pages/Shared/Header/Styles/NotificationPanel.css";
import notification from "../../../assets/Notification.svg"  
import notificationAlarm from "../../../assets/NotificationAlarm.svg"
const NotificationPanel = ({alarmed}) => {
    return(
        <div id="notification-panel">
            <img id="notification-panel-icon" src={notification} alt="Notification" style={{display: alarmed ? "none" : "block"}}/>
            <img id="notification-panel-icon-alarmed" src={notificationAlarm} alt="NotificationAlarm" style={{display: alarmed ? "block" : "none"}}/>
        </div>
    )
}
export default NotificationPanel