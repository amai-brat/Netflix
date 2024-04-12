import ReviewItem from "./ReviewItem.jsx";
import {useEffect, useState} from "react";
import ReactPaginate from "react-paginate";
import styles from './styles/paginatedReview.module.css';
const testReviews =  [
    {
        id: 1,
        user: {
            id: 1,
            avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
            name: "Артурия Пендрагон"
        },
        score: 5,
        writtenAt: "2021-10-10",
        likesScore: 10,
        text: "Отличный фильм",
        isPositive: true,
        comments: [
            {
                id: 1,
                user: {
                    id: 2,
                    avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
                    name: "Александр"
                },
                text: "Согласен",
                writtenAt: "2021-10-10"
            },
            {
                id: 2,
                user: {
                    id: 2,
                    avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
                    name: "Александр"
                },
                text: "Согласен",
                writtenAt: "2021-10-10"
            },
            {
                id: 3,
                user: {
                    id: 2,
                    avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
                    name: "Александр"
                },
                text: "Согласен",
                writtenAt: "2021-10-10"
            }
        ]
    },
    {
        id: 2,
        user: {
            id: 1,
            avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
            name: "Артурия Пендрагон"
        },
        score: 5,
        writtenAt: "2021-10-10",
        likesScore: 10,
        isPositive: false,
        text: "Отличный фильм",
        comments: [
            {
                id: 1,
                user: {
                    id: 2,
                    avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
                    name: "Александр"
                },
                text: "Согласен",
                writtenAt: "2021-10-10"
            }
        ]
    },
    {
        id: 3,
        user: {
            id: 1,
            avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
            name: "Артурия Пендрагон"
        },
        score: 5,
        writtenAt: "2021-10-10",
        likesScore: 10,
        isPositive: true,
        text: "Отличный фильм",
        comments: [
            {
                id: 1,
                user: {
                    id: 2,
                    avatar: "https://wallpapers.com/images/hd/saber-fate-stay-night-ql4w5fa4bkis4bnp.jpg",
                    name: "Александр"
                },
                text: "Согласен",
                writtenAt: "2021-10-10"
            }
        ]
    }
    
    
]
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
    const [items, setItems] = useState([]);
    const [itemOffset, setItemOffset] = useState(0);
    const [totalCount, setTotalCount] = useState(0);
    const [error, setError] = useState(null);
    const endOffset = itemOffset + itemsPerPage;
    useEffect( () => {
        //TODO: раскомментировать после того как будет готов сервер, убрать тестовые данные
        /*const fetchData = async () => {
            try{
                const resp = await fetch(`http://localhost:8000/reviews/${contentId}/?offset=${itemOffset}&limit=${itemsPerPage}&sort=${sort}`);
                const data = await resp.json();
                if (resp.ok){
                    setItems(data.reviews);
                    setTotalCount(data.totalCount);
                } else {
                    setError(data.message);
                }
            } catch (e) {
                setError("Не удалось загрузить данные ");
            }
        };*/
        setItems(testReviews.slice(itemOffset,endOffset));
        setTotalCount(testReviews.length);
        // fetchData();
    },[itemOffset, itemsPerPage, sort, contentId]);
    const handlePageClick = (event) => {
        const newOffset = event.selected * itemsPerPage;
        setItemOffset(newOffset);
    };
    const pageCount = Math.ceil(totalCount / itemsPerPage);
    return (
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
    );
}

export default PaginatedReviews; 