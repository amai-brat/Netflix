import {Route, Routes, useLocation} from "react-router-dom";
import Main from "./Pages/Main/Main.jsx";
import MainContent from "./Pages/MainContent/MainContent.jsx";
import PersonalInfoTab from "./Pages/PersonalAccount/PersonalInfoTab/PersonalInfoTab.jsx";
import FavouritesTab from "./Pages/PersonalAccount/FavouritesTab/FavouritesTab.jsx";
import PersonalReviewsTab from "./Pages/PersonalAccount/PersonalReviewsTab/PersonalReviewsTab.jsx";
import SubscriptionsTab from "./Pages/PersonalAccount/SubscriptionsTab/SubscriptionsTab.jsx";
import SelectionContent from "./Pages/SelectionContent/SelectionContent.jsx";
import SignUpSignIn from "./Pages/SignUp&SignIn/SignUpSignIn.jsx";
import Subscriptions from "./Pages/Subscriptions/Subscriptions.jsx";
import ViewContent from "./Pages/ViewContent/ViewContent.jsx";
import Header from "./Pages/Shared/Header/Header.jsx";
import GeneralPart from "./Pages/PersonalAccount/GeneralPart/GeneralPart.jsx";
import Error404 from "./Pages/Error/Error404.jsx";
import "/src/Pages/Shared/Styles/App.css";
import { SubscriptionsManagement } from './Pages/Admin/Subscriptions/SubscriptionsManagement.jsx';
import {ToastContainer} from "react-toastify";
import AdminContent from "./Pages/Admin/Content/AdminContent.jsx";
import {ProtectedRoute} from "./Pages/Shared/Security/ProtectedRoute.jsx";
import SupportChat from "./Pages/Shared/SupportChat/SupportChat.jsx";
import {useEffect} from "react";
import * as signalR from "@microsoft/signalr";
import {baseSupportUrl} from "./httpClient/baseUrl.js";
import {useDataStore} from "./store/dataStoreProvider.jsx";
import SupportTabWrapper from "./Pages/PersonalAccount/SupportTab/SupportTabWrapper.jsx";

function App() {
    
    const location = useLocation();
    const store = useDataStore()

    useEffect(() => {
        //TODO: заменить url если отличаeтся
        const supportConnection = new signalR.HubConnectionBuilder()
            .withUrl(baseSupportUrl + "hub/messages", {accessTokenFactory: () => {
                    return sessionStorage.getItem("accessToken");
                }})
            .configureLogging(signalR.LogLevel.Information)
            .build();

        supportConnection.start().then(() => {
            store.setSupportConnection(supportConnection)
        }).catch(err => console.error(err))
    }, []);
    
    return (
        <>
            <ToastContainer theme={"dark"} position={"bottom-center"}/>
            {location.pathname !== "/" && !location.pathname.includes("signin") 
                && location.pathname !== "/signup" && <Header/>}
            {location.pathname !== "/" && !location.pathname.includes("signin")
                && location.pathname !== "/signup" && <SupportChat/>}
            <Routes>
                <Route path="/" element={<Main/>}/>
                <Route path="MainContent" element={<MainContent/>}/>
                <Route path={"/PersonalAccount"} element={<ProtectedRoute roles={["user", "support", "admin"]}/>}>
                    <Route path={"/PersonalAccount"} element={<GeneralPart/>}>
                        <Route index element={<PersonalInfoTab/>}/>
                        <Route path="PersonalInfoTab" element={<PersonalInfoTab/>}/>
                        <Route path="FavouritesTab" element={<FavouritesTab/>}/>
                        <Route path="PersonalReviewsTab" element={<PersonalReviewsTab/>}/>
                        <Route path="SubscriptionsTab" element={<SubscriptionsTab/>}/>
                        <Route path={"/PersonalAccount/SupportTab"} element={<ProtectedRoute roles={["support"]}/>}>
                            <Route path={"/PersonalAccount/SupportTab"} element={<SupportTabWrapper/>}/>
                        </Route>
                    </Route>
                </Route>
                <Route path={"/PersonalAccount"} element={<ProtectedRoute roles={["user","admin"]}/>}>
                    <Route path={"admin/subscriptions"} element={<SubscriptionsManagement/>}></Route>
                    <Route path={"admin/content"} element={<AdminContent/>}></Route>
                </Route>
                <Route path="SelectionContent" element={<SelectionContent/>}/>
                <Route path="signup" element={<SignUpSignIn formType="signup"/>}/>
                <Route path="signin" element={<SignUpSignIn formType="signin"/>}>
                    <Route path="google"/>
                    <Route path="vk"/>
                </Route>
                <Route path="Subscriptions" element={<Subscriptions/>}/>
                <Route path="ViewContent/:id" element={<ViewContent/>}/>
                <Route path="*" element={<Error404/>}/>
            </Routes>
        </>
    )
}

export default App
