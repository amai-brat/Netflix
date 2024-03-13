import {useNavigate} from "react-router-dom";

const NotificationPopUpPanel = ({notifications}) =>{
    const navigate = useNavigate()
    const navigateToViewContent = (id) =>{
        navigate("/ViewContent/" + id)
    }
    return(
        <div id="notification-pop-up-panel">
            {notifications.map((notification) => (
                <div onClick={() => {navigateToViewContent(notification.ContentId)}}>
                    <label>{notification.Message}</label>
                    <label>{notification.DateTime}</label>
                </div>
            ))}
        </div>
    )
}
export default NotificationPopUpPanel