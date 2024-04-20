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
const Header = () => {
    const [notifications, setNotifications] = useState([])
    const [alarmed, setAlarmed] = useState(false)
    const [user, setUser] = useState(undefined)
    
    useEffect(() => {
        let isUserAuth = false
        const getCurrentUserDataAsync = async () => {
            try{
                //TODO: Указать действительный url запроса и body с query при необходимости
                const response = await fetch("https://localhost:5000/GetCurrentUserData")
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
        getCurrentUserDataAsync().then(()=>{
                if(!isUserAuth){
                    return
                }
                //Todo: Установить действительный url
                const connection = new signalR.HubConnectionBuilder()
                    .withUrl("http://localhost:5000/NotificationHub", {accessTokenFactory: () => user})
                    .configureLogging(signalR.LogLevel.Information)
                    .build();
    
                connection.start().then(() => {}).catch(err => console.error(err))
                connection.on("ReceiveMessage", (notification) => {
                    setAlarmed(true)
                    setNotifications([...notifications, notification])
                });
            }
        )
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