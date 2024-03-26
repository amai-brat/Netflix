import {useEffect, useState} from 'react'

import styles from './styles/styles.module.css'
import Entry from "./Entry.jsx";
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
    //TODO: обработать аутентификацию.
    //TODO: сделать правильный url
    //TODO: убрать тестовые данные
    const [subscriptions, setSubscriptions] = useState([])
    const [response, setResponse] = useState(null)
    const [isDataFetching, setDataFetching] = useState(false)

    useEffect(() => {
        const fetchSubscriptions = async () => {
            try {
                setDataFetching(true)
                const response = await fetch('https://localhost:5000/subscriptions', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });
                setDataFetching(false)
                if (!response.ok) {
                    setResponse(`Ошибка: ${response.statusText}`)
                    return;
                }
                const data = await response.json();
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
                {subscriptionsForTests.map((value, index) =>
                    <div className={styles.subscriptionBlock}>
                        <Entry
                            key={index}
                            data={value}
                        ></Entry>
                    </div>
                )}
            </div>
        </>
    )
}
export default SubscriptionsTab