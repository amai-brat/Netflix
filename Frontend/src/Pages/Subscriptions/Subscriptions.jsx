import { useState } from 'react'
import pageStyles from "./styles/page.module.scss";
import {SubscriptionInfo} from "./components/SubscriptionInfo.jsx";
import Modal from "react-modal";
import {BankCardForm} from "./components/BankCardForm.jsx";

const Subscriptions = () => {
    const subscriptions = [
        {
            id: 1,
            name: "Netflix Kids",
            price: "300 рублей в месяц",
            infos: [
                "Любой контент с рейтингом 12+ и ниже",
                "FullHD качество",
                "Отмена в любое время",
                "Неизмеримое удовольствие"
            ],
            isCurrentPurchased: true
        },
        {
            id: 2,
            name: "Netflix",
            price: "300 bucks в месяц",
            infos: [
                "Любой контент с рейтингом 12+ и ниже",
                "FullHD качество",
                "Отмена в любое время",
                "Неизмеримое удовольствие"
            ],
            isCurrentPurchased: false
        },
        {
            id: 3,
            name: "Netflix Pro",
            price: "300 bucks в месяц",
            infos: [
                "Любой контент с рейтингом 12+ и ниже",
                "FullHD качество",
                "Отмена в любое время",
                "Неизмеримое удовольствие"
            ],
            isCurrentPurchased: false
        }
    ];

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
    Modal.setAppElement('#root');

    return (
        <div className={pageStyles.subscriptionsPageWrapper}>
            <div></div>
            <div className={pageStyles.subscriptionList}>
                {subscriptions.map((subscription, index) => (
                    <SubscriptionInfo 
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