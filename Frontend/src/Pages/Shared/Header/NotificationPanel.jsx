import "/src/Pages/Shared/Header/Styles/NotificationPanel.css";
const NotificationPanel = ({alarmed}) => {
    return(
        <div id="notification-panel">
            <img id="notification-panel-icon" src="/src/assets/Notification.svg" alt="Notification" style={{display: alarmed ? "none" : "block"}}/>
            <img id="notification-panel-icon-alarmed" src="/src/assets/NotificationAlarm.svg" alt="NotificationAlarm" style={{display: alarmed ? "block" : "none"}}/>
        </div>
    )
}
export default NotificationPanel