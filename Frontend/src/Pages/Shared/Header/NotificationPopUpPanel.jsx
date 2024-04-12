import {useNavigate} from "react-router-dom";
import "/src/Pages/Shared/Header/Styles/NotificationPopUpPanel.css";
import {baseUrl} from "../HttpClient/baseUrl.js";

const NotificationPopUpPanel = ({notifications}) =>{
    const navigate = useNavigate()
    const navigateToViewContent = (id) =>{
        navigate("/ViewContent/" + id)
    }
    return(
        <div id="notification-pop-up-panel">
            {notifications.map((notification, index) => (
                <div key={index} className="notification-info" onClick={() => {
                    fetch(baseUrl + "comment/set/readed?notificationId=" + notification.id, {
                        method: "post"
                    })
                    navigateToViewContent(notification.comment.review.contentId)
                }}>
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