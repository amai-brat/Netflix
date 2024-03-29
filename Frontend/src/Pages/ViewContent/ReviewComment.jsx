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
                    <span>{comment.writtenAt}</span>
                </div>
            </div>
            <div className={styles.reviewText}>
                  <span className={styles.text}>
                      {comment.text + " Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ad adipisci amet asperiores, assumenda autem culpa eos fugiat id, ipsam neque nihil non obcaecati odit quod recusandae, rerum tempora? Dolore, dolorum!" + " Lorem ipsum dolor sit amet, consectetur adipisicing elit. Aut consectetur, cumque debitis dolore modi quia totam ut. Excepturi incidunt, inventore molestias omnis placeat quisquam tempora. Adipisci dolorem et laboriosam rem."}
                  </span>
            </div>
        </div>
    )
}
export default ReviewComment