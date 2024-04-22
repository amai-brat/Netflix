import styles from './styles/page.module.scss';
import {Section} from "./components/Section.jsx";
import { register } from 'swiper/element/bundle';
import {PromoSlider} from "./components/PromoSlider.jsx";
import {useEffect, useState} from "react";
import Map from "./components/map/Map.jsx";
import {contentService} from "../../services/content.service.js";
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
            const {response, data} = await contentService.getSections();
            if (response.ok) {
                setSectionsData(data);
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
                    <p>Ближайшие кинотеатры</p>
                    <Map/>
                </div>
            </div>
        </div>
    )
}

export default MainContent