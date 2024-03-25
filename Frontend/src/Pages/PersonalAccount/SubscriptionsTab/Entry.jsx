import React, {useState} from "react";
import styles from "./styles/styles.module.css";
import Modal from 'react-modal';
import ConfirmationModal from "./ConfirmationModal.jsx";
const Entry = ({data}) => {
    const [isOpened, setIsOpened] = useState(false)
    const [modalIsOpen, setIsOpen] = React.useState(false);
    const [dataFetching, setDataFetching] = React.useState(false);
    const [response, setResponse] = React.useState(null);
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
    async function cancelSubscription(subscriptionName) {
        setDataFetching(true);
        try {
            const response = await fetch('http://localhost:5000/unsubscribe', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ subscriptionName }),
            });
            if (response.ok) {
                setResponse(`Успех`);
            } else {
                setResponse(`Ошибка: ${response.statusText}`);
            }
        } catch (error) {
            setDataFetching(false);
            setResponse(error.message);
        }
    }
    return (
        <>
            <div className={styles.folded}>
                <label>
                    <input type={"checkbox"} onClick={() => openSubscription()}/>
                    <span className={styles.buttonUnfold}
                          style={stateStyles}></span>
                </label>
                <span className={styles.top}>{data.name}</span>
                <span className={styles.bottom}>Куплено: {data.boughtAt}</span>
            </div>
            <div className={styles.unfolded} style={{display: isOpened ? "block" : "none"}}>
                <div className={styles.line}></div>
                <span>Истекает: {data.expiresAt}</span>
                <ul>
                    {data.info.map((value, index) =>
                        <li key={`${data.name}-${index}`}>{value}</li>
                    )}
                </ul>
                <button className={styles.denyButton} onClick={openModal}>Отказаться</button>
                <ConfirmationModal
                    isOpen={modalIsOpen}
                    onRequestClose={closeModal}
                    onConfirm={() => cancelSubscription(data.name)}
                    isDataFetching={dataFetching}
                    response={response}
                ></ConfirmationModal>
            </div>
        </>
    );
};
export default Entry;