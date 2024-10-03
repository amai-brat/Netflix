import { useState } from 'react';
import styles from '../styles/subscriptionCard.module.scss';
import editImg from '../images/pen.svg';
import removeImg from '../../../../assets/Cross.svg';
import Modal from "react-modal";
import {DeletionConfirmation} from "./DeletionConfirmation.jsx";

export const SubscriptionCard = ({ subscription, setSubscriptionId, setContentType }) => {
    const [open, setOpen] = useState(false);
    const [deleteModalOpen, setDeleteModalOpen] = useState(false);
    
    function handleEditButtonClick(event) {
        event.stopPropagation();
        setSubscriptionId(subscription.id);
        setContentType('edit');
    }
    
    function handleDeleteButtonClick(event) {
        event.stopPropagation();
        setDeleteModalOpen(true);
    }

    return (
        <div className={styles.card} data-id={subscription.id}>
            <div className={styles.title} onClick={() => setOpen(!open)}>
                <p>{subscription.name}</p>
                {open && <div className={styles.buttons}>
                    <img src={editImg} alt={"Edit"} width={30} height={30} 
                         onClick={handleEditButtonClick}/>
                    <img src={removeImg} alt={"Del"} width={30} height={30}
                         onClick={handleDeleteButtonClick}/>
                </div>}
            </div>
            {open && 
                <div className={styles.content}>
                    <p>Описание: {subscription.description}</p>
                    <p>Максимальное разрешение: {subscription.maxResolution}</p>
                    <p>Цена: {subscription.price}</p>
                </div>
            }
            <Modal
                isOpen={deleteModalOpen}
                onRequestClose={() => setDeleteModalOpen(false)}
                style={modalStyles}>
                    <DeletionConfirmation subscription={subscription} setModalIsOpen={setDeleteModalOpen}/>
            </Modal>
        </div>
    );
};

const modalStyles = {
    content: {
        textAlign: "center",
        position: "absolute",
        top: "20vh",
        left: "40vw",
        right: "40vw",
        bottom: "auto",
        background: 'rgba(40, 40, 40, 0.8)',
        overflow: 'auto',
        borderRadius: '1em',
        border: "0",
    },
    overlay: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)'
    }
};