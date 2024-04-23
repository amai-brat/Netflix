import styles from "./styles/ReviewComment.module.css";

const ReviewComment = ({comment}) => {
    
    return (
        <div className={styles.reviewItem}>
            <div className={styles.reviewHeader}>
                <div className={styles.userInfo}>
                    {comment.user.avatar && <img src={comment.user.avatar} alt="" className={styles.avatar}/>}
                    <span className={styles.username}>{comment.user.name}</span>
                </div>
                <div className={styles.dateLikesComments}>
                    <span>{comment.writtenAt.toLocaleString().slice(0, 10)}</span>
                </div>
            </div>
            <div className={styles.reviewText}>
                  <span className={styles.text}>
                      {comment.text}
                  </span>
            </div>
        </div>
    )
}
export default ReviewComment