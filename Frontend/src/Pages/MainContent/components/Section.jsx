import styles from '../styles/section.module.scss';
import {Link} from "react-router-dom";
import rightArrow from '../../../assets/RightArrow.svg';
import leftArrow from '../../../assets/LeftArrow.svg';
import 'swiper/css';
import { register } from 'swiper/element/bundle';
register();

export const Section = ( { sectionData } ) => {
    let filterType = 0;
    switch (sectionData.name.toLowerCase()) {
        case "фильмы": filterType = 1; break;
        case "мультфильмы": filterType = 2; break;
        case "сериалы": filterType = 3; break;
    }

    return (
        <section className={styles.pageSection}>

            <div className={styles.pageSectionContainer}>
                <Link className={styles.link} to={"/SelectionContent"} state={{filter: {type: filterType}}}>
                    <p className={styles.sectionName}>{sectionData.name}</p>
                    <img className={styles.arrow} src={rightArrow} width={20} height={20} alt={">"}/>
                </Link>
                <div className={styles.swiperWrapper}>
                    <div className={"prev-button-" + sectionData.name}>
                        <img src={leftArrow} width={30} height={60} alt={"<"}/>
                    </div>
                    <swiper-container
                        navigation-next-el={".next-button-" + sectionData.name}
                        navigation-prev-el={".prev-button-" + sectionData.name}
                        slides-per-view="auto"
                        space-between={20}
                        >
                        {sectionData.contents.map((content, index) => (
                            <swiper-slide key={index}>
                                <Link to={`/ViewContent/${content.id}`} className={styles.card}>
                                    <img src={content.posterUrl} width={160} height={240} alt={"Постер"}/>
                                    <p>{content.name}</p>
                                </Link>
                            </swiper-slide>
                        ))}
                    </swiper-container>
                    <div className={"next-button-" + sectionData.name}>
                        <img src={rightArrow} width={30} height={60} alt={">"}/>
                    </div>
                </div>
            </div>
        </section>
    )
};