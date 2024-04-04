import React, {useEffect, useMemo, useState} from "react";
import ReactDOM from "react-dom";

const Map = () => {
    const [data, setData] = useState({ymaps3: null, reactify: null})

    const getMapComponents = async () => {
        await ymaps3.ready;

        const ymaps3Reactify = await ymaps3.import('@yandex/ymaps3-reactify');
        const reactify = ymaps3Reactify.reactify.bindTo(React, ReactDOM);

        setData({
            ymaps3: ymaps3,
            reactify: reactify
        })
    }
    
    useEffect(() => {
        getMapComponents()
    }, []);

    if(data.ymaps3 === null){
        return null
    }

    const {YMap, YMapDefaultSchemeLayer, YMapDefaultFeaturesLayer, YMapMarker} = data.reactify.module(data?.ymaps3)
    
    return(
        <YMap location={{center: [25.229762, 55.289311], zoom: 9}} mode="vector" >
            <YMapDefaultSchemeLayer />
            <YMapDefaultFeaturesLayer />

            <YMapMarker coordinates={[25.229762, 55.289311]} draggable={true}>
                <section>
                    <h1 style={{color:"white"}}>You can drag this header</h1>
                </section>
            </YMapMarker>
        </YMap>
    )
}

export default Map;