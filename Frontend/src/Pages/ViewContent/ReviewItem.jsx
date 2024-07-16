import styles from './styles/ReviewItem.module.css'
import heart from './Images/heart_jon_phillips_01.png'
import comment from './Images/comment-svgrepo-com.svg'
import closeCross from './Images/icons8-close-96.svg'
import {toast} from "react-toastify";
import {useContext, useState} from "react";
import Modal from "react-modal";
import ReviewComment from "./ReviewComment.jsx";
import {useDataStore} from "../../store/dataStoreProvider.jsx";
import {commentService} from "../../services/comment.service.js";
import {ReviewsContext} from "./ReviewsContext.js";
import {contentService} from "../../services/content.service.js";
import {authenticationService} from "../../services/authentication.service.js";
import moderatorImg from "../../assets/moderator.svg";
import crossImg from "../../assets/Cross.svg";
import {moderatorService} from "../../services/moderator.service.js";
import {adminUserService} from "../../services/admin.user.service.js";
import defaultUserIcon from "../../assets/default.png"

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
const ReviewItem = ({review, customStyles, notOpenModal}) => {
    const {setReviewsChanged} = useContext(ReviewsContext);
    const [modalOpen, setModalOpen] = useState(false)
    const [commentText, setCommentText] = useState('')
    const [userIcon, setUserIcon] = useState(!review.user.avatar ? defaultUserIcon : review.user.avatar)
    const store = useDataStore()
    const setDefaultUserImg = () => {
        setUserIcon(defaultUserIcon)
    }
    const handleTextChange = (event) => {
        setCommentText(event.target.value)
    }
    const stylesCombined = {
        ...customStyles,
        backgroundColor: review.isPositive? "#09830b": "#c00c00"
    }
    const closeModal = () => {
        setModalOpen(false)
        document.body.style.overflow = "unset"
    }
    const openReviewModal = () => {
        setModalOpen(true)
        document.body.style.overflow = "hidden"
    }
    const sendComment = async () => {
        try {
            const {response, data} = await commentService.createComment(review.id, {Text: commentText});
            if (response.ok) {
                try {await store.data.connection.invoke("NotifyAboutCommentAsync", data);} catch (e) {console.log(e)}
                setReviewsChanged(true);
                toast.success("Комментарий отправлен", {
                    position: "bottom-center"
                })
            } else {
                toast.error(data.message, {
                    position: "bottom-center"
                })
            }
        } catch (e){
            toast.error("Почему-то не получилось отправить комментарий, попробуйте позже", {
                position: "bottom-center"
            })
        } finally {
            closeModal();
        }
    }
    const likeReview = async () => {
        try{
            const {response, data} = await contentService.likeReview(review.id);
            if (response.ok){
                setReviewsChanged(true);
                
                toast.success(data === true ? "Лайк поставлен" : "Лайк удалён", {
                    position: "bottom-center"
                })
            } else{
                toast.error(data.message, {
                    position: "bottom-center"
                })
            }
        } catch (e){
            toast.error(e.message, {
                position: "bottom-center"
            })
        }
    }
    
    async function handleReviewDeleteClick() {
        try {
            const {response, data} = await moderatorService.deleteReview(review.id);
            if (response.ok) {
                setReviewsChanged(true);
                toast.success("Рецензия удалена", {
                    position: "bottom-center"
                })
            }
            else {
                toast.error(data, {
                    position: "bottom-center"
                })
            }
        } catch (e) {
            toast.error("Ошибка")
        }
    }

    async function handleMakeModeratorClick() {
        try {
            const {response, data} = await adminUserService.makeModerator(review.user.id);
            if (response.ok) {
                toast.success("Пользователь стал модератором", {
                    position: "bottom-center"
                })
            }
            else {
                toast.error(data, {
                    position: "bottom-center"
                })
            }
        } catch (e) {
            toast.error("Ошибка")
        }
    }
    const formatDateTime = (dateTimeStr) => {
        const date = new Date(dateTimeStr);

        const year = date.getFullYear();
        const month = date.getMonth() + 1;
        const day = date.getDate();
        const hours = date.getHours();
        const minutes = date.getMinutes().toString().padStart(2, '0');

        const formattedDate = `${hours}:${minutes}, ${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
        return formattedDate;
    }
    return(
      <>
          <div className={styles.reviewItem} style={stylesCombined}>
              <div className={styles.reviewHeader}>
                  <div className={styles.userInfo}>
                      <img src={userIcon} alt="" className={styles.avatar} onError={setDefaultUserImg}/>
                      <span className={styles.username}>{review.user.name}</span>
                      <div className={styles.authorizedButtons}>
                          {authenticationService.isCurrentUserAdmin() &&
                            <img src={moderatorImg} alt={"Moderator Icon"}
                                 className={styles.makeModeratorButton}
                                 width={25} height={25}
                                 title={"Сделать модератором"}
                                 onClick={handleMakeModeratorClick}/>}
                          {authenticationService.isCurrentUserModerator() && 
                            <img src={crossImg} alt={"Delete"}
                                 className={styles.deleteButton}
                                 width={25} height={25}
                                 title={"Удалить рецензию"}
                                 onClick={handleReviewDeleteClick}/>}
                      </div>
                  </div>
                  <div className={styles.dateLikesComments}>
                      <span>{formatDateTime(review.writtenAt)}</span>
                      <span className={styles.commentsLikes}>
                          {review.comments.length} <img src={comment} alt={"Комментариев:"} className={styles.comment}
                                                        onClick={notOpenModal ? null : openReviewModal}/>
                          {review.likesScore} <img src={heart} alt={"Лайков:"} className={styles.heart}
                                                   onClick={likeReview}/>
                      </span>
                  </div>
              </div>
              <div className={styles.reviewText} onClick={notOpenModal ? null : openReviewModal}>
                  <span className={styles.text}>
                      {review.text}
                  </span>
                  <span className={styles.score}>{review.score}/10</span>
              </div>
              <Modal
                isOpen={modalOpen}
                onRequestClose={closeModal}
                style={modalStyles}
                contentLabel="Example Modal"
                appElement={document.getElementById('root')}>
                  <div className={styles.modalReview}>
                      <div className={styles.modalHeader}>
                          <span style={{width: "2rem", flexShrink: "10"}}></span>
                          <h1>Детали ревью</h1>
                          <img src={closeCross} alt={"Закрыть"} className={styles.closeCross} onClick={closeModal}/>
                      </div>
                      <ReviewItem review={review}
                                  customStyles={{width: "100%", height: "fit-content", fontSize: "150%",}}
                                  notOpenModal={true}
                      ></ReviewItem>
                      {review.comments.map((comment) => {
                          return (
                              <ReviewComment key={comment.id} comment={comment}></ReviewComment>
                          )
                      })}
                      <h2>Оставить комментарий</h2>
                      <textarea
                        style={{color: "black"}}
                          placeholder="Ответить в комментарии"
                          className={styles.commentForm} onChange={handleTextChange}
                          rows={10}>
                      </textarea>
                      <button className={styles.sendReview} onClick={sendComment}>Отправить</button>
                  </div>
              </Modal>
          </div>
      </>
    );
}
export default ReviewItem