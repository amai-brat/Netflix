import { SubscriptionsSidebar } from './components/SubscriptionsSidebar';
import styles from './styles/page.module.scss';
import React, {useEffect, useState} from "react";
import {SubscriptionForm} from "./components/SubscriptionForm.jsx";
import {baseUrl} from "../../Shared/HttpClient/baseUrl.js";
import {SubscriptionsContext} from "./components/SubscriptionsContext.js";
import {getSubscriptions} from "./httpClient.js";

export const SubscriptionsManagement = () => {
    const [subscriptions, setSubscriptions] = useState([]);
    
    useEffect(() => {
        (async() => {
            const res = await getSubscriptions();
            if (res && res.ok) {
                setSubscriptions(res.data);
            }
        })();
    }, []);

    const [contentType, setContentType] = useState('none');
    const [subscriptionId, setSubscriptionId] = useState('');
    
    const contents = {
        'edit': <SubscriptionForm subscription={subscriptions.find(x => x.id === subscriptionId)}/>,
        'new': <SubscriptionForm/>,
        'none': <div/>
    }
    
    return (
      <SubscriptionsContext.Provider value={{subscriptions, setSubscriptions}}>
        <div className={styles.container}>
            <div className={styles.leftSidebar}>
                <SubscriptionsSidebar subscriptions={subscriptions} 
                                      setContentType={setContentType}
                                      setSubscriptionId={setSubscriptionId}
                ></SubscriptionsSidebar>
            </div>
            <div className={styles.content}>
                {contents[contentType]}
            </div>
        </div>
      </SubscriptionsContext.Provider>)
};