import { SubscriptionsSidebar } from './components/SubscriptionsSidebar';
import styles from './styles/page.module.scss';
import {useEffect, useState} from "react";
import {SubscriptionForm} from "./components/SubscriptionForm.jsx";
import {SubscriptionsContext} from "./components/SubscriptionsContext.js";
import {adminSubscriptionService} from "../../../services/admin.subscription.service.js";

export const SubscriptionsManagement = () => {
    const [subscriptions, setSubscriptions] = useState([]);
    
    useEffect(() => {
        (async() => {
            const {response, data} = await adminSubscriptionService.getSubscriptions();
            if (response.ok) {
                setSubscriptions(data);
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