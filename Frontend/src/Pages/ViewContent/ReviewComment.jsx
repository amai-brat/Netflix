import styles from "./styles/ReviewComment.module.css";
import crossImg from '../../assets/Cross.svg';
import moderatorImg from '../../assets/moderator.svg';
import {authenticationService} from "../../services/authentication.service.js";
import {moderatorService} from "../../services/moderator.service.js";
import {toast} from "react-toastify";
import {adminUserService} from "../../services/admin.user.service.js";

const ReviewComment = ({comment}) => {
  async function handleCommentDeleteClick() {
    try {
      const {response, data} = await moderatorService.deleteComment(comment.id);
      if (response.ok) {
        toast.success("Комментарий удалён", {
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
      const {response, data} = await adminUserService.makeModerator(comment.user.id);
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

  return (
        <div className={styles.reviewItem}>
            <div className={styles.reviewHeader}>
              <div className={styles.userInfo}>
                {comment.user.avatar && <img src={comment.user.avatar} alt="" className={styles.avatar}/>}
                <span className={styles.username}>{comment.user.name}</span>
                <div className={styles.authorizedButtons}>
                  {authenticationService.isCurrentUserAdmin() && <img src={moderatorImg} alt={"Moderator Icon"}
                                                                      className={styles.makeModeratorButton}
                                                                      width={25} height={25}
                                                                      title={"Сделать модератором"}
                                                                      onClick={handleMakeModeratorClick}/>}
                  {authenticationService.isCurrentUserModerator() && <img src={crossImg} alt={"Delete"}
                                                                          className={styles.deleteButton}
                                                                          width={25} height={25}
                                                                          title={"Удалить комментарий"}
                                                                          onClick={handleCommentDeleteClick}/>}
                </div>
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