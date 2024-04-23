import {useEffect, useState} from 'react'
import ContentInfo from "./ContentInfo.jsx";
import styles from './styles/ViewContent.module.css';
import ContentPlayer from "./contentPlayer.jsx";
import Reviews from "./Reviews.jsx";
import {useParams} from "react-router-dom";
import {contentService} from "../../services/content.service.js";
const ViewContent = () => {
    let { id } = useParams();
    const [contentData,setContentData] = useState(null)
    const [error, setError] = useState(null)
    
    useEffect(() => {
        async function fetchData() {
            try {
                const {response, data } = await contentService.getContentInfo(id);
                if (response.ok) {
                    setContentData(data);
                } else{
                    setError(data.message)
                }
            } catch (e) {
                setError(e.message)
            }
        }
        fetchData();
    }, [id]);
    return (
        <>
            {error && <h1 style={
                {textAlign: 'center'}
            }>{error}</h1>}
            {contentData &&
            <div className={styles.generalContainer}>
                <div className={styles.wholePageContainer}>
                    <ContentInfo contentData={contentData}/>
                    <ContentPlayer contentId={id} contentType={contentData.contentType}
                                   seasonInfos={contentData.seasonInfos} />
                    <Reviews contentId={id}/>
                </div>
            </div>
            }
        </>
    )
}

export default ViewContent