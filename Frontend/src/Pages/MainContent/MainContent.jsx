import styles from './styles/page.module.scss';
import {Section} from "./components/Section.jsx";
import { register } from 'swiper/element/bundle';
import {PromoSlider} from "./components/PromoSlider.jsx";
import {useEffect, useState} from "react";
register();


const MainContent = () => {

    const [sectionsData, setSectionsData] = useState([
        {
            name: "Фильмы",
            contents: [
                {
                    id: 1,
                    name: "Фильм 1",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 2,
                    name: "Фильм 2",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 3,
                    name: "Фильм 3",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 4,
                    name: "Фильм 4",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 5,
                    name: "Фильм 5",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 6,
                    name: "Фильм 6",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 7,
                    name: "Фильм 7",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 8,
                    name: "Фильм 8",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                }
            ]
        },
        {
            name: "Мультфильмы",
            contents: [
                {
                    id: 1,
                    name: "Мульт 1",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 2,
                    name: "Мульт 2",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 3,
                    name: "Мульт 3",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 4,
                    name: "Мульт 4",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 5,
                    name: "Мульт 5",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },
                {
                    id: 6,
                    name: "Мульт 6",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                },

            ]
        },
        {
            name: "Сериалы",
            contents: [
                {
                    id: 1,
                    name: "Сериал 1",
                    posterUrl: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
                }
            ]
        }
    ]);

    useEffect(() => {
        // TODO: настоящий url запроса
        (async () => {
            const response = await fetch('http://localhost:8080/getSectionsOfContent');
            if (response.ok)
            {
                setSectionsData(await response.json());
            }
        })()

    }, []);

    return (
        <div className={styles.pageWrapper}>
            <PromoSlider></PromoSlider>
            <div className={styles.sectionsList}>
                {sectionsData.map(((sectionData, index) => (
                    <Section sectionData={sectionData} key={index}/>
                )))}
            </div>
            <div className={styles.mapWrapper}>
                <div className={styles.innerWrapper}>
                    <p>Доступные кинотеатры</p>
                    <iframe
                        src="https://yandex.ru/map-widget/v1/?ll=49.136538%2C55.786424&mode=poi&poi%5Bpoint%5D=49.123375%2C55.791550&poi%5Buri%5D=ymapsbm1%3A%2F%2Forg%3Foid%3D1763683699&z=13.54"
                        allowFullScreen="true"
                    ></iframe>
                </div>
            </div>
        </div>
    )
}

export default MainContent