import {useState} from "react";
import reviewStyle from "../styles/review.module.scss";

export const ReviewAccordion = ({review}) => {
  const [isActive, setIsActive] = useState(false);

  return (
    <div className={reviewStyle.accordionItem + " " + (isActive ? reviewStyle.reviewBlockOpened : reviewStyle.reviewBlock)} data-id={review.id}>
      <div className={reviewStyle.accordionTitle} onClick={() => setIsActive(!isActive)}>
        <div className={(reviewStyle.rating) + ' ' + (review.isPositive ? reviewStyle.positiveReview : reviewStyle.negativeReview)}>
          <p>{review.rating}</p>
        </div>
        <div className={reviewStyle.reviewInfo}>
          <p className={reviewStyle.contentName}>{review.contentName}</p>
          <p className={reviewStyle.reviewName}>{review.name}</p>
          <p className={reviewStyle.date}>{review.updatedAt}</p>
        </div>
        {!isActive && <p className={reviewStyle.reviewTextBeginning}>{review.text.slice(0, 150) + (review.text.length > 150 ? "..." : "")}</p>}
      </div>
      {isActive && <div className={reviewStyle.accordionContent}>
        <p className={reviewStyle.reviewText}>{review.text}</p>
      </div>}
    </div>
  );
}
