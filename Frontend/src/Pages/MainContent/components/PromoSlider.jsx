import Swiper from 'swiper/bundle';
import rightArrow from '../../../assets/RightArrow.svg';
import leftArrow from '../../../assets/LeftArrow.svg';
import 'swiper/css';
import { register } from 'swiper/element/bundle';
import {useEffect} from "react";
register();

export const PromoSlider = () => {

    const id = 1;
    return (
        <>
            <swiper-container
                navigation-next-el={".custom-next-button-" + id}
                navigation-prev-el={".custom-prev-button-" + id}
                slides-per-view={"1"}
            >
                <swiper-slide>Slide 1</swiper-slide>
                <swiper-slide>Slide 2</swiper-slide>
                <swiper-slide>Slide 3</swiper-slide>
            </swiper-container>
            <div className={"custom-next-button-" + id}>aa</div>
            <div className={"custom-prev-button-" + id}>bb</div>
        </>
    );
}