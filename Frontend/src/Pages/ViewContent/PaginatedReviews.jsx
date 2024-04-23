import ReviewItem from "./ReviewItem.jsx";
import {useEffect, useState} from "react";
import ReactPaginate from "react-paginate";
import styles from './styles/paginatedReview.module.css';
import {ReviewsContext} from "./ReviewsContext.js";
import {contentService} from "../../services/content.service.js";

function Items({newItems}) {
    return (
        <>
            {newItems.map((review) =>
                <ReviewItem review={review} key={review.id}/>)
            }
        </>
    )
}
const PaginatedReviews = ({contentId,itemsPerPage,sort}) => {
    const [reviewsChanged, setReviewsChanged] = useState(false);
    const [items, setItems] = useState([]);
    const [itemOffset, setItemOffset] = useState(0);
    const [totalCount, setTotalCount] = useState(0);
    const [error, setError] = useState(null);
    const endOffset = itemOffset + itemsPerPage;
    
    useEffect(() => {
        fetchData();
    },[itemOffset, itemsPerPage, sort, contentId]);
    
    useEffect(() => {
        if (!reviewsChanged) return;
        fetchData();
        setReviewsChanged(false);
    }, [reviewsChanged]);
    
    const handlePageClick = (event) => {
        const newOffset = event.selected * itemsPerPage;
        setItemOffset(newOffset);
    };
    
    const fetchData = async () => {
        try{
            const {response: reviewResp, data: revs} = await contentService.getReviews(contentId, itemOffset, itemsPerPage, sort);
            const {response: countResp, data: count} = await contentService.getReviewsCount(contentId);

            if (reviewResp.ok){
                setItems(revs);
            } else {
                setError(revs.message);
            }

            if (countResp.ok) {
                setTotalCount(count);
            }
        } catch (e) {
            setError("Не удалось загрузить данные ");
        }
    };
    
    const pageCount = Math.ceil(totalCount / itemsPerPage);
    return (
        <ReviewsContext.Provider value={{reviewsChanged, setReviewsChanged}}>
            <>
            {error && <h3 className={styles.errorMessage}>{error}</h3>}
            {items.length === 0 && <h3>Пока нет рецензий</h3>}
            {items.length > 0 && 
                <>
                    <Items newItems={items}/>
                    <ReactPaginate
                        breakLabel="..."
                        pageCount={pageCount}
                        pageRangeDisplayed={4}
                        onPageChange={handlePageClick}
                        marginPagesDisplayed={1}
                        renderOnZeroPageCount={null}
                        nextLabel={null}
                        previousLabel={null}
                        previousClassName={styles.previous}
                        nextClassName={styles.next}
                        containerClassName={styles.pagination}
                        pageLinkClassName={styles.pageLinkClassName}
                        activeLinkClassName={styles.activeLinkClassName}
                        pageClassName={styles.pageClassName}
                        breakClassName={styles.breakClassName}
                    />
                </>
            }
            </>
        </ReviewsContext.Provider>
    );
}


export default PaginatedReviews; 