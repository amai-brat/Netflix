import ReviewItem from "./ReviewItem.jsx";
import {useEffect, useState} from "react";
import ReactPaginate from "react-paginate";
import styles from './styles/paginatedReview.module.css';
const testReviews =  [
    {
        id:1,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: true,
        likes: 100,
        comments: [
            {
                id:1,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:2,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:3,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:4,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:5,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            }
        ]
    },{
        id:2,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: true,
        likes: 100,
        comments: [
            {
                id:6,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:7,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:8,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:9,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            },
            {
                id:10,
                name: "Ilnur",
                text: "my answer",
                writtenAt: "2021-09-01",
                likes: 100
            }
        ]
    },{
        id:3,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: true,
        likes: 100,
        comments: []
    },
    {
        id:14,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: true,
        likes: 100,
        comments: []
    },
    {
        id:15,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: true,
        likes: 100,
        comments: []
    },
    {
        id:16,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },
    {
        id:18,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },
    {
        id:17,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:20,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:21,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:22,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:23,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:24,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:25,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:26,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:27,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:28,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:29,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:30,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:31,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:32,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:444,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:353,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:313,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:833,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:373,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:433,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:323,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:332,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:3654,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:3354,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:35343,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:3573,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:3863,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:33765,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:345323,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2021-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:31113,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2019-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },{
        id:32223,
        name: "Almaz",
        text: "my commet",
        score: 8,
        writtenAt: "2020-09-01",
        isPositive: false,
        likes: 100,
        comments: []
    },
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