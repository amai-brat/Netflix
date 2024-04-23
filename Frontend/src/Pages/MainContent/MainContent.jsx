import styles from './styles/page.module.scss';
import {Section} from "./components/Section.jsx";
import { register } from 'swiper/element/bundle';
import {PromoSlider} from "./components/PromoSlider.jsx";
import {useEffect, useState} from "react";
import Map from "./components/map/Map.jsx";
import {contentService} from "../../services/content.service.js";
register();


const MainContent = () => {

    const [sectionsData, setSectionsData] = useState([]);

    useEffect(() => {
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