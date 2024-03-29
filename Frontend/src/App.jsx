import {Component, useState} from 'react'
import {BrowserRouter, Route, Routes, useLocation} from "react-router-dom";
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

function App() {
    const location = useLocation();

    return (
        <>
            {location.pathname !== "/" && location.pathname !== "/signin" 
                && location.pathname !== "/signup" && <Header/>}
            <Routes>
                <Route path="/" element={<Main/>}/>
                <Route path="MainContent" element={<MainContent/>}/>
                <Route path="PersonalAccount" element={<GeneralPart/> }>
                    <Route path="PersonalInfoTab" element={<PersonalInfoTab/>}/>
                    <Route path="FavouritesTab" element={<FavouritesTab/>}/>
                    <Route path="PersonalReviewsTab" element={<PersonalReviewsTab/>}/>
                    <Route path="SubscriptionsTab" element={<SubscriptionsTab/>}/>
                </Route>
                <Route path="SelectionContent" element={<SelectionContent/>}/>
                {/*Здесь должен быть Route для админовской части*/}
                <Route path="signup" element={<SignUpSignIn formType="signup"/>}/>
                <Route path="signin" element={<SignUpSignIn formType="signin"/>}/>
                <Route path="Subscriptions" element={<Subscriptions/>}/>
                <Route path="ViewContent/:id" element={<ViewContent/>}/>
                <Route path="*" element={<Error404/>}/>
            </Routes>
        </>
    )
}

export default App
