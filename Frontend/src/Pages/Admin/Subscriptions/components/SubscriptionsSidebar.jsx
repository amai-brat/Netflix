import { SubscriptionCard } from "./SubscriptionCard";
import styles from '../styles/sidebar.module.scss';
import plus from '../../../../assets/plus.svg';

export const SubscriptionsSidebar = ({ subscriptions, setSubscriptionId, setContentType }) => {
  return (
    <div className={styles.sidebar}>
      <div className={styles.header}>
        <h1>Subscriptions</h1>
        <img src={plus} alt={"New"} height={30} width={30} onClick={() => setContentType('new')}/>
      </div>
      {subscriptions.map((subscription, index) => (
        <SubscriptionCard key={index} subscription={subscription} 
                          setSubscriptionId={setSubscriptionId} 
                          setContentType={setContentType}></SubscriptionCard>
      ))}
    </div>
  );
};

