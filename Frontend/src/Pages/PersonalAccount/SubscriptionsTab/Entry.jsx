import React, {useState} from "react";
import styles from "./styles/styles.module.css";
import Modal from 'react-modal';
import ConfirmationModal from "./ConfirmationModal.jsx";
const Entry = ({data}) => {
    const [isOpened, setIsOpened] = useState(false)
    const [modalIsOpen, setIsOpen] = React.useState(false);

    function openModal() {
        setIsOpen(true);
    }

    function closeModal() {
        setIsOpen(false);
    }

    const stateStyles = {
        transform: `rotate(${isOpened ? 180 : 0}deg)`
    }

    function openSubscription() {
        setIsOpened(!isOpened)
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
                ></ConfirmationModal>
            </div>
        </>
    );
};
export default Entry;