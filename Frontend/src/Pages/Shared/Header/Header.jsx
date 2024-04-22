import {useEffect, useState} from 'react'
import {useNavigate, useNavigation} from "react-router-dom"
import ClientPopUpPanel from "./ClientPopUpPanel.jsx"
import NavigatePanel from "./NavigatePanel.jsx"
import SearchPanel from "./SearchPanel.jsx"
import ClientPanel from "./ClientPanel.jsx"
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx"
import "/src/Pages/Shared/Header/Styles/Header.css"
import NotificationPanel from "./NotificationPanel.jsx";
import NotificationPopUpPanel from "./NotificationPopUpPanel.jsx";
import * as signalR from "@microsoft/signalr";
import {baseUrl} from "../../../httpClient/baseUrl.js";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
import {userService} from "../../../services/user.service.js";
import {notificationService} from "../../../services/notification.service.js";
const Header = () => {
    const [notifications, setNotifications] = useState([])
    const [alarmed, setAlarmed] = useState(false)
    const [user, setUser] = useState(undefined)
    const store = useDataStore()
    
    useEffect(() => {
        let isUserAuth = false
        const getCurrentUserDataAsync = async () => {
            try{
                const {response, data} = await userService.getPersonalInfo();
                if(response.ok){
                    setUser({name: data.nickname, icon: data.profilePictureUrl })
                    isUserAuth = true
                }else{
                    setUser(null)
                }
            }
            catch (error){
                setUser(null)
                console.error(error)
            }
        }
        
        const getNotificationHistory = async () => {
            try{
                const {response, data} = await notificationService.getNotificationHistory();
                if(response.ok){
                    if(data.filter(x => !x.readed).length !== 0){
                        setAlarmed(true)   
                    }
                    setNotifications(data.filter(x => !x.readed));
                }else{
                    setAlarmed(false)
                    setNotifications([])
                }
            }
            catch (error){
                setAlarmed(false)
                setNotifications([])
                console.error(error)
            }
        }
        
        getCurrentUserDataAsync().then(() => {
            getNotificationHistory().then(()=>{
                    if(!isUserAuth){
                        return
                    }
                    const connection = new signalR.HubConnectionBuilder()
                        .withUrl(baseUrl + "hub/notification", {accessTokenFactory: () => {
                            return sessionStorage.getItem("accessToken");
                            }})
                        .configureLogging(signalR.LogLevel.Information)
                        .build();

                    connection.start().then(() => {
                        store.setConnection(connection)
                    }).catch(err => console.error(err))
                
                    connection.on("ReceiveNotification", (notification) => {
                        setAlarmed(true)
                        setNotifications(notifications => [...notifications, notification])
                    });
                    
                    connection.on("DeleteNotification", (notificationId) => {
                        setNotifications(nots => nots.filter(x => x.id !== +notificationId))
                    });
                }
            )  
        })
    }, []);
    
    const navigate = useNavigate()
    const navigateToSignIn = () => {
        navigate("/signin");
    }
    
    return (
        <header>
            <NavigatePanel/>
            <SearchPanel/>
            <div id="client-info-panel-and-sign-up-button-container">
                <input id="sign-up-header-button" type="button" value="Войти" onClick={navigateToSignIn} style={
                    {display: user === null || user === undefined ? "block" : "none"}
                }/>
                <div id="client-info-panel" style={{display: user === null || user === undefined ? "none" : "flex"}}>
                    <div id="notification-panel-wrapper" onClick={() => {setAlarmed(false)}}>
                        <ComponentWithPopUp
                            Component = {() => <NotificationPanel alarmed={alarmed}/>}
                            PopUp = {() => <NotificationPopUpPanel notifications={notifications}/>}
                            id = {"pop-up-notification"}
                        />
                    </div>
                    <ComponentWithPopUp
                        Component = {() => <ClientPanel user={user}/>}
                        PopUp = {() => <ClientPopUpPanel user={user}/>}
                        id = {"pop-up-client"}
                    />
                </div>
            </div>
        </header>
    )
}

export default Header