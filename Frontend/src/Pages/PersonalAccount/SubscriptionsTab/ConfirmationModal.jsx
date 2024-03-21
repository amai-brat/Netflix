import Modal from "react-modal";
import React from "react";
import styles from "./styles/modal.module.css";
const customStyles = {
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        transform: 'translate(-50%, -50%)',
        background: '#000',
        overflow: 'auto',
        borderRadius: '4px'
    },
    overlay: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)'
    }
};
const ConfirmationModal = ({ isOpen, onRequestClose}) => {
    return (
        <Modal
            isOpen={isOpen}
            style={customStyles}
            onRequestClose={onRequestClose}
            contentLabel="Example Modal"
        >
            <div className={styles.modalContent}>
                <h2>Точно отписаться?</h2>
                <div className={styles.modalButtons}>
                    <button onClick={onRequestClose} className={styles.cancelButton}>Cancel</button>
                    <button onClick={onRequestClose} className={styles.confirmButton}>Confirm
                    </button>
                </div>
            </div>
        </Modal>
    );
}
export default ConfirmationModal;