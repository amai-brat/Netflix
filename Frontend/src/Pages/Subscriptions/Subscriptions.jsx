import {useEffect, useState} from 'react'
import pageStyles from "./styles/page.module.scss";
import {SubscriptionInfo} from "./components/SubscriptionInfo.jsx";
import Modal from "react-modal";
import {BankCardForm} from "./components/BankCardForm.jsx";
import {subscriptionService} from "../../services/subscription.service.js";

const Subscriptions = () => {
    const [purchasedSubscriptions, setPurchasedSubscriptions] = useState([]);
    const [subscriptions, setSubscriptions] = useState([]);

    useEffect(() => {
        (async() => {
            try {
                const {response, data} = await subscriptionService.getAllSubscriptions();
                if (response.ok) {
                    setSubscriptions(data);
                }
            } catch (e) {
                console.log(e);
            }

            try {
                const {response, data} = await subscriptionService.getCurrentSubscriptions();
                if (response.ok) {
                    setPurchasedSubscriptions(data);
                }
            } catch (e) {
                console.log(e);
            }
        })()
    }, []);

    useEffect(() => {
        if (!subscriptions) return;
        const purchasedIds = purchasedSubscriptions.map(x => x.subscriptionId); 
        for (let i = 0; i < subscriptions.length; i++) {
            subscriptions[i].isCurrentPurchased = purchasedIds.includes(subscriptions[i].id);
        }
    }, [subscriptions, purchasedSubscriptions]);
    
    const modalStyles = {
        content: {
            display: "grid",
            gridTemplateColumns: "1fr 2fr",
            gridAutoFlow: "column",
            position: "absolute",
            top: "20vh",
            left: "20vw",
            right: "20vw",
            bottom: "auto",
            background: 'rgba(40, 40, 40, 0.8)',
            overflow: 'auto',
            borderRadius: '1em',
            border: "0"
        },
        overlay: {
            backgroundColor: 'rgba(0, 0, 0, 0.8)'
        }}
    const [modalIsOpen, setModalIsOpen] = useState(false);
    const [subscriptionId, setSubscriptionId] = useState(0);

    return (
        <div className={pageStyles.subscriptionsPageWrapper}>
            <div></div>
            <div className={pageStyles.subscriptionList}>
                {subscriptions.map((subscription, index) => (
                    <SubscriptionInfo 
                        key={index}
                        subscription={subscription} 
                        showPurchase={true}
                        setModalIsOpen={setModalIsOpen}
                        setSubscriptionId={setSubscriptionId}/>
                ))}
            </div>
            <Modal
                style={modalStyles}
                isOpen={modalIsOpen}
                onRequestClose={() => setModalIsOpen(false)}>
                <div>
                    <p className={pageStyles.modalText}>Вы покупаете: </p>
                    <SubscriptionInfo
                        subscription={subscriptions.find(x => x.id === +subscriptionId)}
                        showPurchase={false}/>
                </div>
                <BankCardForm subscriptionId={subscriptionId}/>
            </Modal>
        </div>
    )
}

export default Subscriptions