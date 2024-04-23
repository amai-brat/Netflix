import {useEffect, useState} from 'react'
import background from './styles/background.module.css'
import {NavLink, Outlet, useLocation} from "react-router-dom";
import content from './styles/content.module.css'
import tabContent from './styles/tabContent.module.css'

const GeneralPart = ({component: Component}) => {
    const [currentTab, setCurrentTab] = useState(0);
    const tabs = [
        {name: "Личные данные", link: "PersonalInfoTab"},
        {name: "Избранное", link: "FavouritesTab"},
        {name: "Рецензии", link: "PersonalReviewsTab"},
        {name: "Подписки", link: "SubscriptionsTab"},
        {name: "Контент", link: "admin/content"}
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
            <div className={background.background}>
                <div className={content.content}>
                    <div className={content.navigation}>
                        {tabs.map((tab, index) =>
                            <div key={tab.link}
                                 className={`${content.tab} ${index === currentTab ? content.active : ''}`}
                                 onClick={() => tabClicked(index)}>
                                <NavLink to={tab.link} key={tab.link}>{tab.name}</NavLink>
                            </div>
                        )}
                    </div>
                    <div className={tabContent.tabContent}>
                        <Outlet></Outlet>
                    </div>
                </div>
            </div>
        </>
    )
}

export default GeneralPart