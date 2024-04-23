import styles from './styles/Reviews.module.css';
import {useState} from "react";
import PaginatedReviews from "./PaginatedReviews.jsx";
import Modal from 'react-modal';
import {toast} from "react-toastify";
import {contentService} from "../../services/content.service.js";
const modalStyles = {
    content: {
        top: '50%',
        left: '50%',
        right: 'auto',
        bottom: 'auto',
        marginRight: '-50%',
        transform: 'translate(-50%, -50%)',
        width:'70%',
        height:'70%',
        backgroundColor: 'rgba(0, 0, 0, 0.9)',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        zIndex: '1000'
    },
    overlay:{
        backgroundColor: 'rgba(0, 0, 0, 0.9)',
        zIndex: '1000'
    }
}
const Reviews = ({contentId}) => {
    const [filterOpened, setFilterOpened] = useState(false);
    const [filter, setFilter] = useState('newest');
    const [isGivingReview, setIsGivingReview] = useState(false);
    const [currentStarIndex, setCurrentStarIndex] = useState(0);
    const [reviewText, setReviewText] = useState('');
    const handleTextChange = (event) => {
        setReviewText(event.target.value);
    };
    const openModal = () => {
        setIsGivingReview(true);
        document.body.style.overflow = 'hidden';
    }
    const closeModal = () => {
        setIsGivingReview(false);
        document.body.style.overflow = 'unset';
    }
    const filterClicked = () => {
        setFilterOpened(!filterOpened);
    }
    const setFilterOption = (option) => {
        setFilter(option);
    }
    function notifyAboutResult(isSuccess, message){
        if (isSuccess) {
            toast.success(message, {
                position: "bottom-center"
            });
        } else {
            toast.error(message, {
                position: "bottom-center"
            });
        }
    }
    const sendReview = async () => {
        try {
            const {response, data} = await contentService.createReview({
                contentId: contentId, 
                score: currentStarIndex + 1, 
                text: reviewText,
                isPositive: currentStarIndex + 1 > 5
            });
            
            if (response.ok) {
                notifyAboutResult(true, "Рецензия создана");
            } else {
                notifyAboutResult(false, data.message);
            }
        } catch (e){
            notifyAboutResult(false, e.message);
        } finally {
            closeModal();
        }
    }
    return (
        <>
            <div className={styles.reviews}>
                <div className={styles.writeReviewAndFilter}>
                    {/*знатно наговнокодил*/}
                    <div style={{width: "156px", flexShrink:"10"}}></div>
                    <div className={styles.writeReview} onClick={openModal}>Напиши свою рецензию!</div>
                    <Modal
                        isOpen={isGivingReview}
                        onRequestClose={closeModal}
                        style={modalStyles}
                        contentLabel="Example Modal"
                        appElement={document.getElementById('root')}>
                        <h1>Дайте оценку</h1>
                        <button className={styles.closeModal} onClick={closeModal}></button>
                        <div className="stars">
                            {[...Array(10)].map((_, index) => (
                                <svg onClick={() => setCurrentStarIndex(index)} key={index} width="3rem" height="3rem"
                                     viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path
                                        d="M9.15316 5.40838C10.4198 3.13613 11.0531 2 12 2C12.9469 2 13.5802 3.13612 14.8468 5.40837L15.1745 5.99623C15.5345 6.64193 15.7144 6.96479 15.9951 7.17781C16.2757 7.39083 16.6251 7.4699 17.3241 7.62805L17.9605 7.77203C20.4201 8.32856 21.65 8.60682 21.9426 9.54773C22.2352 10.4886 21.3968 11.4691 19.7199 13.4299L19.2861 13.9372C18.8096 14.4944 18.5713 14.773 18.4641 15.1177C18.357 15.4624 18.393 15.8341 18.465 16.5776L18.5306 17.2544C18.7841 19.8706 18.9109 21.1787 18.1449 21.7602C17.3788 22.3417 16.2273 21.8115 13.9243 20.7512L13.3285 20.4768C12.6741 20.1755 12.3469 20.0248 12 20.0248C11.6531 20.0248 11.3259 20.1755 10.6715 20.4768L10.0757 20.7512C7.77268 21.8115 6.62118 22.3417 5.85515 21.7602C5.08912 21.1787 5.21588 19.8706 5.4694 17.2544L5.53498 16.5776C5.60703 15.8341 5.64305 15.4624 5.53586 15.1177C5.42868 14.773 5.19043 14.4944 4.71392 13.9372L4.2801 13.4299C2.60325 11.4691 1.76482 10.4886 2.05742 9.54773C2.35002 8.60682 3.57986 8.32856 6.03954 7.77203L6.67589 7.62805C7.37485 7.4699 7.72433 7.39083 8.00494 7.17781C8.28555 6.96479 8.46553 6.64194 8.82547 5.99623L9.15316 5.40838Z"
                                        fill={index <= currentStarIndex ? "#3688ff" : "#1C274C"}
                                        className={styles.star}/>
                                </svg>
                            ))}
                        </div>
                        <textarea value={reviewText}
                                  placeholder="Поделитесь вашими мыслями. Пары предложений будет достаточно"
                                  className={styles.reviewText} onChange={handleTextChange}></textarea>
                        <button className={styles.sendReview} onClick={sendReview}>Отправить</button>
                    </Modal>
                    <div className={styles.filterContainer}>
                        {filterOpened &&
                            <ul className={styles.filterOptions}>
                                <li onClick={() => setFilterOption("newest")}
                                    className={filter === "newest" ? styles.active : ``}>Новейшие
                                </li>
                                <li onClick={() => setFilterOption("positive")}
                                    className={filter === "positive" ? styles.active : ``}>Положительные
                                </li>
                                <li onClick={() => setFilterOption("negative")}
                                    className={filter === "negative" ? styles.active : ``}>Отрицательные
                                </li>
                                <li onClick={() => setFilterOption("scoredesc")}
                                    className={filter === "scoredesc" ? styles.active : ``}>По оценкам
                                </li>
                                <li onClick={() => setFilterOption("likesDesc")}
                                    className={filter === "likesDesc" ? styles.active : ``}>По лайкам(убывание)
                                </li>
                            </ul>}
                        <div className={styles.filter} onClick={filterClicked}>Отсортировать</div>
                    </div>
                </div>
                <div className={styles.reviewList}>
                <h2>Рецензии</h2>
                <PaginatedReviews contentId={contentId} itemsPerPage={4} sort={filter}/>
            </div>
            </div>
        </>
    );
}
export default Reviews