import Modal from "react-modal";
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
const ConfirmationModal = ({ isOpen, onRequestClose, onConfirm, isDataFetching, response}) => {
    return (
        <Modal
            isOpen={isOpen}
            style={customStyles}
            onRequestClose={onRequestClose}
            onConfirm={onConfirm}
        >
            <div className={styles.modalContent}>
                <h2>Точно отписаться?</h2>
                {isDataFetching && <p>Отписываемся...</p>}
                {response && <p>{response}</p>}
                <div className={styles.modalButtons}>
                    <button onClick={onRequestClose} className={styles.cancelButton}>Назад</button>
                    <button onClick={onConfirm} className={styles.confirmButton}>Да
                    </button>
                </div>
            </div>
        </Modal>
    );
}
export default ConfirmationModal;