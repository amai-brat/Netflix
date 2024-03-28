import rightArrow from '../../../assets/RightArrow.svg';
import leftArrow from '../../../assets/LeftArrow.svg';
import styles from '../styles/promoSilder.module.scss';
import 'swiper/css';
import { register } from 'swiper/element/bundle';
import {Link} from "react-router-dom";
import {useEffect, useState} from "react";
register();

export const PromoSlider = () => {
    const [promoImages, setPromoImages] = useState([
        {
            id: 1,
            url: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
        },
        {
            id: 2,
            url: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
        },
        {
            id: 3,
            url: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
        },
        {
            id: 4,
            url: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
        },
        {
            id: 5,
            url: "https://image.openmoviedb.com/kinopoisk-images/1898899/8ef070c9-2570-4540-9b83-d7ce759c0781/orig"
        }
    ]);

    useEffect(() => {
        // TODO: настоящий url запроса
        (async () => {
            const response = await fetch('http://localhost:8080/getPromos');
            if (response.ok)
            {
                setPromoImages(await response.json());
            }
        })()
    }, []);
    return (
        <div className={styles.promoSlider}>
            <div className={styles.promoSliderContainer}>
                <div className={"prev-button-promo-slider"}>
                    <img src={leftArrow} width={30} height={60} alt={"<"}/>
                </div>
                <swiper-container
                    navigation-next-el={".next-button-promo-slider"}
                    navigation-prev-el={".prev-button-promo-slider"}
                    slides-per-view={"1.3"}
                    slides-per-group={"1"} loop={"true"}
                    centered-slides={"true"}
                    space-between={"40"}
                    watch-slides-progress={"true"}>
                    {promoImages.map((promo, index) => (
                            <swiper-slide key={index}>
                                <Link to={"/ViewContent/" + promo.id}>
                                    <img src={promo.url} alt={"promo"}/>
                                </Link>
                            </swiper-slide>
                        ))}
                </swiper-container>

                <div className={"next-button-promo-slider"}>
                    <img src={rightArrow} width={30} height={60} alt={">"}/>
                </div>
            </div>
        </div>
    );
}