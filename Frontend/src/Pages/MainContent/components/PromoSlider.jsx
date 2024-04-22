import rightArrow from '../../../assets/RightArrow.svg';
import leftArrow from '../../../assets/LeftArrow.svg';
import styles from '../styles/promoSilder.module.scss';
import 'swiper/css';
import { register } from 'swiper/element/bundle';
import {Link} from "react-router-dom";
import {useEffect, useState} from "react";
import {contentService} from "../../../services/content.service.js";
register();

export const PromoSlider = () => {
    const [promoImages, setPromoImages] = useState([]);

    useEffect(() => {
        (async () => {
            const {response, data} = await contentService.getPromos();
            if (response.ok)
            {
                setPromoImages(data);
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
                                    <img src={promo.posterUrl} alt={"promo"}/>
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