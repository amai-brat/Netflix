import {Component, useState} from 'react'
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

function App() {
    const location = useLocation();
    
    return (
        <>
            {location.pathname !== "/" && <Header />}
            <Routes>
                <Route path="/" element={Main()} />
                <Route path="MainContent" element={MainContent()} />
                <Route path="PersonalAccount/PersonalInfoTab" element={GeneralPart(PersonalInfoTab)} />
                <Route path="PersonalAccount/FavouritesTab" element={GeneralPart(FavouritesTab)} />
                <Route path="PersonalAccount/PersonalReviewsTab" element={GeneralPart(PersonalReviewsTab)} />
                <Route path="PersonalAccount/SubscriptionsTab" element={GeneralPart(SubscriptionsTab)} />
                <Route path="SelectionContent" element={SelectionContent()} />
                {/*Здесь должен быть Route для админовской части*/}
                <Route path="SignUpSignIn" element={SignUpSignIn()} />
                <Route path="Subscriptions" element={Subscriptions()} />
                <Route path="ViewContent/:id" element={ViewContent()} />
                <Route path="*" element={Error404()}/>
            </Routes>
        </>
    )
}

export default App
