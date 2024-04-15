import styles from '../styles/deletionConfirmation.module.scss';
import {baseUrl} from "../../../Shared/HttpClient/baseUrl.js";

export const DeletionConfirmation = ({ subscription, setModalIsOpen }) => {
    async function handleDeleteButtonClick() {
        const serverMessageElement = document.getElementById("server-message");
        try {
            const response = await fetch(`${baseUrl}admin/subscription/delete/${subscription.id}`, {
                method: "DELETE",
                headers: {
                    // TODO: auth token
                    "Authorization": "Bearer [token]"
                }
            });
            if (response.ok) {
                serverMessageElement.innerHTML = "Успешно удалено";
                setTimeout(() => setModalIsOpen(false), 500);
            } else {
                serverMessageElement.innerHTML = "Не получилось удалить: " + await response.text();
            }
        } catch (e) {
            serverMessageElement.innerHTML = "Ошибка";
            console.log(e);
        }
    }

    return (
        <div className={styles.confirmation}>
            <h2>Действительно хотите удалить?</h2>
            <div className={styles.buttons}>
                <button onClick={handleDeleteButtonClick}>Да</button>
                <button onClick={() => setModalIsOpen(false)}>Нет</button>
            </div>
            <span id={"server-message"}></span>
        </div>
    );
}