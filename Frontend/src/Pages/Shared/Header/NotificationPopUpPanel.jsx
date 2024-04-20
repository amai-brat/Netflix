import {useNavigate} from "react-router-dom";
import "/src/Pages/Shared/Header/Styles/NotificationPopUpPanel.css";

const NotificationPopUpPanel = ({notifications}) =>{
    const navigate = useNavigate()
    const navigateToViewContent = (id) =>{
        navigate("/ViewContent/" + id)
    }
    return(
        <div id="notification-pop-up-panel">
            {notifications.map((notification, index) => (
                <div key={index} className="notification-info" onClick={() => {navigateToViewContent(notification.ContentId)}}>
                    <label className="notification-message">{notification.Message}</label>
                    <label className="notification-date">{notification.DateTime}</label>
                </div>
            ))}
        </div>
    )
}
export default NotificationPopUpPanel