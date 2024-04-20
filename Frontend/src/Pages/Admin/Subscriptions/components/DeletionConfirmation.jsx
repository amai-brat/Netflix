import styles from '../styles/deletionConfirmation.module.scss';
import {baseUrl} from "../../../Shared/HttpClient/baseUrl.js";
import {toast} from "react-toastify";
import {useContext} from "react";
import {SubscriptionsContext} from "./SubscriptionsContext.js";

export const DeletionConfirmation = ({ subscription, setModalIsOpen }) => {
    const {setSubscriptions} = useContext(SubscriptionsContext);
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
                toast.success("Успешно удалено", {
                    position: "bottom-center"
                });
                setSubscriptions(subscriptions => subscriptions.filter(x => x.id !== subscription.id))
                setTimeout(() => setModalIsOpen(false), 500);
            } else {
                toast.error("Не получилось удалить: " + await response.text(), {
                    position: "bottom-center"
                })
            }
        } catch (e) {
            toast.error("Ошибка", {
                position: "bottom-center"
            })
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
        </div>
    );
}