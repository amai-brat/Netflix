import styles from './styles/Reviews.module.css';

const Reviews = ({reviews}) => {
    return (
        <>
            <div className={styles.reviews}>
                <div className={styles.writeReview}>Напиши свою рецензию!</div>
                <div className={styles.reviewList}>
                    <h2>Рецензии</h2>
                </div>
            </div>
        </>
    );
}
export default Reviews