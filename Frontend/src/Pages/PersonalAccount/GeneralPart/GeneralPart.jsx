import {useEffect, useState} from 'react'
import './styles/background.css'
import {useLocation} from "react-router-dom";
import './styles/content.css'
import './styles/tabContent.css'

const GeneralPart = ({component: Component}) => {
    const [currentTab, setCurrentTab] = useState(0);
    const tabs = ["Личные данные", "Избранное", "Подписки", "Рецензии"];
    const location = useLocation();
    const setInitialTab = () => {
        const pathSegments = location.pathname.split('/');
        const accountSection = pathSegments[pathSegments.indexOf('PersonalAccount') + 1];
        const tabNames = ["PersonalInfoTab", "FavouritesTab", "SubscriptionsTab", "PersonalReviewsTab"];

        const tabIndex = tabNames.findIndex(name => name.toLowerCase() === accountSection?.toLowerCase());
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
                                {tab}
                            </div>
                        )}
                    </div>
                    <div className={"tabContent"}>
                        <Component/>
                    </div>
                </div>
            </div>
        </>
    )
}

export default GeneralPart