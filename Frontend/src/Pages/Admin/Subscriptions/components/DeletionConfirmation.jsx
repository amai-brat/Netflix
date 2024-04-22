import styles from '../styles/deletionConfirmation.module.scss';
import {toast} from "react-toastify";
import {useContext} from "react";
import {SubscriptionsContext} from "./SubscriptionsContext.js";
import {adminSubscriptionService} from "../../../../services/admin.subscription.service.js";

export const DeletionConfirmation = ({ subscription, setModalIsOpen }) => {
    const {setSubscriptions} = useContext(SubscriptionsContext);
    async function handleDeleteButtonClick() {
        try {
            const {response, data} = await adminSubscriptionService.deleteSubscription(subscription.id);
            if (response.ok) {
                toast.success("Успешно удалено", {
                    position: "bottom-center"
                });
                setSubscriptions(subscriptions => subscriptions.filter(x => x.id !== subscription.id))
                setTimeout(() => setModalIsOpen(false), 500);
            } else {
                toast.error("Не получилось удалить: " + data, {
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