import {useNavigate} from "react-router-dom";
import "/src/Pages/Shared/Header/Styles/NotificationPopUpPanel.css";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";

const NotificationPopUpPanel = ({notifications}) =>{
    const navigate = useNavigate()
    const store = useDataStore();
    const navigateToViewContent = (id) =>{
        navigate("/ViewContent/" + id)
    }
    
    const handleNotificationClick = async (notification) => {
        await store.data.connection.invoke("ReadNotification", notification.id)
        navigateToViewContent(notification.comment.review.contentId)
    }
    
    return(
        <div id="notification-pop-up-panel">
            {notifications.map((notification, index) => (
                <div key={index} className="notification-info" onClick={() => handleNotificationClick(notification)}>
                    <label className="notification-message">
                        Ответ от {notification.comment.user.nickname}: {notification.comment.text}
                    </label>
                    <label className="notification-date">{notification.comment.writtenAt.substring(0,10)}</label>
                </div>
            ))}
        </div>
    )
}
export default NotificationPopUpPanel