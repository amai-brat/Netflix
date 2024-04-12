import React, {useEffect, useMemo, useState} from "react"
import ReactDOM from "react-dom"

const Map = () => {
    const [data, setData] = useState({ymaps3: null, reactify: null})
    const [cinemas, setCinemas] = useState([])
    const [currentPos, setCurrentPos] = useState([55.47, 49.6])

    const getMapComponents = async () => {
        await ymaps3.ready;

        const ymaps3Reactify = await ymaps3.import('@yandex/ymaps3-reactify');
        const reactify = ymaps3Reactify.reactify.bindTo(React, ReactDOM);

        setData({
            ymaps3: ymaps3,
            reactify: reactify
        })
    }

    const getCurrentPos = async () => {
        const pos = await ymaps3.geolocation.getPosition()
        setCurrentPos(pos.coords)
        return pos.coords
    }

    const getCinemas = async () => {
        const currentPos = await getCurrentPos();
        const bottomLeft = [currentPos[0] - 0.1, currentPos[1] - 0.1]
        const topRight = [currentPos[0] + 0.1, currentPos[1] + 0.1]

        const cinemas = await ymaps3.search({text: "Кинотеатр", center: currentPos, offset: 0, limit: 20, bounds: [bottomLeft, topRight]})
        setCinemas(cinemas)
    }
    
    useEffect(() => {
        getMapComponents().then(() => {
            getCinemas()    
        })
    }, [])

    if(data.ymaps3 === null){
        return null
    }
    
    const {YMap, YMapDefaultSchemeLayer, YMapDefaultFeaturesLayer, YMapMarker} = data.reactify.module(data?.ymaps3)

    return(
        <YMap 
            location={{center: currentPos, zoom: 13}} 
            mode="vector"
        >
            <YMapDefaultSchemeLayer />
            <YMapDefaultFeaturesLayer />
            {cinemas.map((cinema, index) => 
                <YMapMarker 
                    key={index}
                    coordinates={cinema.geometry.coordinates}
                    draggable={false}
                >
                    <img src="src/assets/Marker.svg" alt="Marker" style={{width:"36px", height:"36px"}}/>
                </YMapMarker>
            )}
        </YMap>
    )
}

export default Map;