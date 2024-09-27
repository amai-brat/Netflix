import React, {useEffect, useState} from "react";
import styles from "./styles/styles.module.css";
import ConfirmationModal from "./ConfirmationModal.jsx";
import {subscriptionService} from "../../../services/subscription.service.js";
import {authenticationService} from "../../../services/authentication.service.js";
const Entry = ({data, setSubscriptions}) => {
    const [isOpened, setIsOpened] = useState(false)
    const [modalIsOpen, setIsOpen] = React.useState(false);
    const [dataFetching, setDataFetching] = React.useState(false);
    const [response, setResponse] = React.useState(null);
    const [subscription, setSubscription] = useState('');
    const stateStyles = {
        transform: `rotate(${isOpened ? 180 : 0}deg)`
    }
    function openModal() {
        setIsOpen(true);
    }

    function closeModal() {
        setResponse(null)
        setIsOpen(false);
        setDataFetching(false)
    }
    
    function openSubscription() {
        setIsOpened(!isOpened)
    }
    async function cancelSubscription(subscriptionId) {
        setDataFetching(true);
        try {
            const {response: unsubResp} = await subscriptionService.unsubscribe(subscriptionId);
            if (unsubResp.ok) {
                setResponse(`Успех`);
                setSubscriptions(subs => subs.filter(x => x.subscriptionId !== subscriptionId));
                await authenticationService.refreshToken();
            } else {
                setResponse(`Ошибка: ${unsubResp.status}`);
            }
        } catch (error) {
            setDataFetching(false);
            setResponse(error.message);
        }
        setDataFetching(false);
    }

    useEffect(() => {
        (async() => {
            try {
                const {response, data: respData} = await subscriptionService.getSubscriptionById(data.subscriptionId);
                if (response.ok) {
                    setSubscription(respData)
                }
            } catch (e) {
                console.log(e)
            }
        })()
    }, []);
    return (
        <>
            <div className={styles.folded}>
                <label>
                    <input type={"checkbox"} onClick={() => openSubscription()}/>
                    <span className={styles.buttonUnfold}
                          style={stateStyles}></span>
                </label>
                <span className={styles.top}>{subscription.name}</span>
                <span className={styles.bottom}>Куплено: {data.boughtAt.toLocaleString().slice(0, 10)}</span>
            </div>
            <div className={styles.unfolded} style={{display: isOpened ? "block" : "none"}}>
                <div className={styles.line}></div>
                <span>Истекает: {data.expiresAt.toLocaleString().slice(0, 10)}</span>
                <ul>
                    <li>Максимальное разрешение: {subscription.max_resolution}</li>
                    <li>Описание: {subscription.description}</li>
                </ul>
                <button className={styles.denyButton} onClick={openModal}>Отказаться</button>
                <ConfirmationModal
                    isOpen={modalIsOpen}
                    onRequestClose={closeModal}
                    onConfirm={() => cancelSubscription(data.subscriptionId)}
                    isDataFetching={dataFetching}
                    response={response}
                ></ConfirmationModal>
            </div>
        </>
    );
};
export default Entry;