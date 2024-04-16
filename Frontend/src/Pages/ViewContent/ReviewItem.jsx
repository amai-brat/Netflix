import styles from './styles/ReviewItem.module.css'
import heart from './Images/red-heart-icon.svg'
import comment from './Images/comment-svgrepo-com.svg'
import closeCross from './Images/icons8-close-96.svg'
import {toast} from "react-toastify";
import {useState} from "react";
import Modal from "react-modal";
import ReviewComment from "./ReviewComment.jsx";
import {baseUrl} from "../Shared/HttpClient/baseUrl.js";
<<<<<<< HEAD
import {useDataStore} from "../../store/dataStoreProvider.jsx";
=======
>>>>>>> dev
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
    const [modalOpen, setModalOpen] = useState(false)
    const [commentText, setCommentText] = useState('')
    const store = useDataStore()
    
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
<<<<<<< HEAD
            const resp = await fetch(baseUrl + "comment/assign?reviewId=" + review.id, {
=======
            const resp = await fetch(baseUrl + "api/comments/add", {
>>>>>>> dev
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                //TODO: добавить токен
                body: JSON.stringify({Text: commentText})
            });
            const body = await resp.json();
            if (resp.ok) {
                store.data.connection.invoke("NotifyAboutCommentAsync", body)
                toast.success(body.message, {
                    position: "bottom-center"
                })
            } else {
                toast.error(body.message, {
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
            //TODO: напистаь правильный юрл
            const resp = await fetch(baseUrl + "api/review/like", {
                method: "post",
                // TODO: написать поля с идентификацией юзера
                body: {reviewId: review.id}
            })
            const body = await resp.json()
            if (resp.ok){
                toast.success(body.message, {
                    position: "bottom-center"
                })
            } else{
                toast.error(body.message, {
                    position: "bottom-center"
                })
            }
        } catch (e){
            toast.error(e.message, {
                position: "bottom-center"
            })
        }
    }
    return(
      <>
          <div className={styles.reviewItem} style={stylesCombined}>
              <div className={styles.reviewHeader}>
                  <div className={styles.userInfo}>
                  {review.user.avatar && <img src={review.user.avatar} alt="" className={styles.avatar}/>}
                  <span className={styles.username}>{review.user.name}</span>
                    </div>
                  <div className={styles.dateLikesComments}>
                      <span>{review.writtenAt}</span>
                      {review.comments.length > 0 &&
                          <span className={styles.commentsLikes}>
                              {review.comments.length} <img src={comment} alt={"Комментариев:"} className={styles.comment} onClick={notOpenModal? null: openReviewModal}/>
                              {review.likesScore} <img src={heart} alt={"Лайков:"} className={styles.heart} onClick={likeReview}/>
                          </span>}
                  </div>
              </div>
              <div className={styles.reviewText} onClick={notOpenModal? null: openReviewModal}>
                  <span className={styles.text}>
                      {review.text +" Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ad adipisci amet asperiores, assumenda autem culpa eos fugiat id, ipsam neque nihil non obcaecati odit quod recusandae, rerum tempora? Dolore, dolorum!" + " Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aut consectetur, cumque debitis dolore modi quia totam ut. Excepturi incidunt, inventore molestias omnis placeat quisquam tempora. Adipisci dolorem et laboriosam rem."}
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