import {useEffect, useState} from 'react'
import './styles/background.css'
import {NavLink, Outlet, useLocation} from "react-router-dom";
import './styles/content.css'
import './styles/tabContent.css'

const GeneralPart = ({component: Component}) => {
    const [currentTab, setCurrentTab] = useState(0);
    const tabs = [
        {name: "Личные данные", link: "PersonalInfoTab"},
        {name: "Избранное", link: "FavouritesTab"},
        {name: "Рецензии", link: "PersonalReviewsTab"},
        {name: "Подписки", link: "SubscriptionsTab"}
    ];
    const location = useLocation();
    const setInitialTab = () => {
        const pathSegments = location.pathname.split('/');
        const accountSection = pathSegments[pathSegments.indexOf('PersonalAccount') + 1];
        const tabIndex = tabs.findIndex(tab => tab.link.toLowerCase() === accountSection?.toLowerCase());
        if (tabIndex !== -1) {
            setCurrentTab(tabIndex);
        }
    };
    useEffect(() => {
        setInitialTab();
    }, [location]);
    function tabClicked(index) {
        setCurrentTab(index);
    }
    return (
        <>
            <div className={"background"}>
                <div className={"content"}>
                    <div className={"navigation"}>
                        {tabs.map((tab, index) =>
                            <div className={`tab ${index === currentTab ? 'active' : ''}`}
                                 onClick={() => tabClicked(index)}>
                                <NavLink to={tab.link} key={tab.link}>{tab.name}</NavLink>
                            </div>
                        )}
                    </div>
                    <div className={"tabContent"}>
                        <Outlet></Outlet>
                    </div>
                </div>
            </div>
        </>
    )
}

export default GeneralPart