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
import {baseUrl} from "../HttpClient/baseUrl.js";
import {useDataStore} from "../../../store/dataStoreProvider.jsx";
const Header = () => {
    const [notifications, setNotifications] = useState([])
    const [alarmed, setAlarmed] = useState(false)
    const [user, setUser] = useState(undefined)
    const store = useDataStore()
    
    useEffect(() => {
        let isUserAuth = false
        const getCurrentUserDataAsync = async () => {
            try{
                const response = await fetch(baseUrl + "user/get-personal-info")
                if(response.ok){
                    const userData = await response.json()
                    setUser({name: userData.Nickname, icon: userData.ProfilePictureUrl })
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
                const response = await fetch(baseUrl + "comment/notifications")
                if(response.ok){
                    const notificationsData = await response.json()
                    if(notificationsData.filter(x => !x.readed).length !== 0){
                        setAlarmed(true)   
                    }
                    setNotifications(notificationsData)
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
                        .withUrl(baseUrl + "hub/notification")
                        .configureLogging(signalR.LogLevel.Information)
                        .build();

                    connection.start().then(() => {
                        store.setConnection(connection)
                    }).catch(err => console.error(err))
                
                    connection.on("ReceiveNotification", (notification) => {
                        setAlarmed(true)
                        setNotifications(notifications => [...notifications, notification])
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