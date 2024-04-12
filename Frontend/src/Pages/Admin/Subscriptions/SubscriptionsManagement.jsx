import { SubscriptionsSidebar } from './components/SubscriptionsSidebar';
import styles from './styles/page.module.scss';
import React, {useEffect, useState} from "react";
import {SubscriptionForm} from "./components/SubscriptionForm.jsx";
import {baseUrl} from "../../Shared/HttpClient/baseUrl.js";

export const SubscriptionsManagement = () => {
    const [subscriptions, setSubscriptions] = useState([]);

    useEffect(() => {
        (async() => {
            try {
                const response = await fetch(baseUrl + "admin/subscriptions/all", {
                    method: "GET",
                    headers: {
                        // TODO: auth token
                        // "Authorization": "Bearer [token]"
                    }
                });

                if (response.ok) {
                    const data = await response.json();
                    setSubscriptions(data);
                }
            } catch (e) {
                console.log(e);
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
        </div>)
};