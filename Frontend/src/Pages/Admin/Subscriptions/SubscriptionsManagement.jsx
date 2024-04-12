import { SubscriptionsSidebar } from './components/SubscriptionsSidebar';
import styles from './styles/page.module.scss';
import React, {useEffect, useState} from "react";
import {SubscriptionForm} from "./components/SubscriptionForm.jsx";

export const SubscriptionsManagement = () => {
    const [subscriptions, setSubscriptions] = useState([]);

    useEffect(() => {
        setSubscriptions([
            {
                id: 1,
                name: "HELP",
                description: "ABOBA",
                maxResolution: 1080,
                price: 300,
                accessibleContents: [
                    {
                        id: 1,
                        name: "BLYYYYA"
                    },
                    {
                        id: 2,
                        name: "FAPAHHH"
                    }
                ]
            },
            {
                id: 2,
                name: "Assasas",
                description: "ABasaOBA",
                maxResolution: 720,
                price: 227,
                accessibleContents: [
                    {
                        id: 3,
                        name: "BLYYYYA"
                    },
                    {
                        id: 4,
                        name: "FAPAHHH"
                    }
                ]
            }
        ]);
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