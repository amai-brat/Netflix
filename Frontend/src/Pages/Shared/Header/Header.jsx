import {useEffect, useState} from 'react'
import {useNavigate, useNavigation} from "react-router-dom"
import ClientPopUpPanel from "./ClientPopUpPanel.jsx"
import NavigatePanel from "./NavigatePanel.jsx"
import SearchPanel from "./SearchPanel.jsx"
import ClientPanel from "./ClientPanel.jsx"
import ComponentWithPopUp from "../PopUpModule/ComponentWithPopUp.jsx"
import "/src/Pages/Shared/Header/Header.css"
import NotificationPanel from "./NotificationPanel.jsx";
import NotificationPopUpPanel from "./NotificationPopUpPanel.jsx";
import * as signalR from "@microsoft/signalr";
const Header = () => {
    const [notifications, setNotifications] = useState([])
    const [alarmed, setAlarmed] = useState(false)
    const [user, setUser] = useState(undefined)
    
    useEffect(() => {
        const getCurrentUserDataAsync = async () => {
            try{
                //TODO: Указать действительный url запроса и body с query при необходимости
                const response = await fetch("https://localhost:5000/GetCurrentUserData")
                if(response.ok){
                    const userData = await response.json()
                    setUser(null)
                }else{
                    setUser(null)
                }
            }
            catch (error){
                setUser({name: "d", icon:"d"})
                console.error(error)
            }
        }
        getCurrentUserDataAsync().then(()=>{
                if(user === null || user === undefined){
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
    const navigateToSignUpSignIn = () => {
        navigate("/SignUpSignIn")
    }
    
    return (
        <header>
            <NavigatePanel/>
            <SearchPanel/>
            <div id="sign-up-header-button" style={{display: user === null ? "block" : "none"}}>
                <input type="button" value="Войти" onClick={navigateToSignUpSignIn}/>
            </div>
            <div style={{display: user === null || user === undefined ? "none" : "block"}}>
                <div onClick={() => {setAlarmed(false)}}>
                    <ComponentWithPopUp
                        Component = {() => <NotificationPanel alarmed={alarmed}/>}
                        PopUp = {() => <NotificationPopUpPanel notifications={notifications}/>}
                    />
                </div>
                <ComponentWithPopUp
                    Component = {() => <ClientPanel user={user}/>}
                    PopUp = {() => <ClientPopUpPanel user={user}/>}
                />
            </div>
        </header>
    )
}

export default Header