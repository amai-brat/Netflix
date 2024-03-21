import { useState } from 'react'
import styles from './styles/styles.module.css'
import Entry from "./Entry.jsx";
const SubscriptionsTab = () => {
    //TODO: обработать аутентификацию.
    //TODO: сделать запрос на получение подписок
    const subscriptions = [
        {
            name: "Netflix Premium",
            expiresAt: "2022-07-01",
            boughtAt: "2021-07-01",
            info: [
                "Просмотр в HD и Ultra HD",
                "Просмотр на 4 устройствах одновременно",
                "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
            ]
        },
        {
            name: "Netflix Premium",
            expiresAt: "2022-07-01",
            boughtAt: "2021-07-01",
            info: [
                "Просмотр в HD и Ultra HD",
                "Просмотр на 4 устройствах одновременно",
                "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
            ]
        },
        {
            name: "Netflix Premium",
            expiresAt: "2022-07-01",
            boughtAt: "2021-07-01",
            info: [
                "Просмотр в HD и Ultra HD",
                "Просмотр на 4 устройствах одновременно",
                "Просмотр на телевизорах, планшетах, смартфонах и компьютерах"
            ]
        }
    ]
    
    return (
        <>
            <div className={styles.container}>
                {subscriptions.map((value, index) =>
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