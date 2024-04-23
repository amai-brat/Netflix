import {useEffect, useState} from 'react'

import styles from './styles/styles.module.css'
import Entry from "./Entry.jsx";
import {subscriptionService} from "../../../services/subscription.service.js";
const subscriptionsForTests = [
    {
        name: "Netflix Premium1",
        expiresAt: "2022-07-01",
        boughtAt: "2021-07-01",
        info: [
            "Просмотр в HD и Ultra HD",
            "Просмотр на 4 устройствах одновременно",
            "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
        ]
    },
    {
        name: "Netflix Premium2",
        expiresAt: "2022-07-01",
        boughtAt: "2021-07-01",
        info: [
            "Просмотр в HD и Ultra HD",
            "Просмотр на 4 устройствах одновременно",
            "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
        ]
    },
    {
        name: "Netflix Premium3",
        expiresAt: "2022-07-01",
        boughtAt: "2021-07-01",
        info: [
            "Просмотр в HD и Ultra HD",
            "Просмотр на 4 устройствах одновременно",
            "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
        ]
    }
]
const SubscriptionsTab = () => {
    const [subscriptions, setSubscriptions] = useState([])
    const [response, setResponse] = useState(null)
    const [isDataFetching, setDataFetching] = useState(false)

    useEffect(() => {
        const fetchSubscriptions = async () => {
            try {
                setDataFetching(true)
                const {response, data} = await subscriptionService.getPurchasedSubscriptions();
                setDataFetching(false)
                if (!response.ok) {
                    setResponse(`Ошибка: ${response.statusText}`)
                    return;
                }
                setSubscriptions(data);
            } catch (error) {
                setDataFetching(false)
                setResponse(error.message)
            }
        };
        fetchSubscriptions();
    }, []);
    
    return (
        <>
            <div className={styles.subscriptionsLoading}>
            {isDataFetching && <div id="subscriptions-loading">Загрузка...</div>}
            {response && <div>{response}</div>}
            </div>
            <div className={styles.container}>
                {subscriptions.map((value, index) =>
                    <div key={index} className={styles.subscriptionBlock}>
                        <Entry
                            data={value}
                            setSubscriptions={setSubscriptions}
                        ></Entry>
                    </div>
                )}
            </div>
        </>
    )
}
export default SubscriptionsTab