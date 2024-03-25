import { useState } from 'react';
import searchIcon from '../../../assets/Search.svg';
import searchStyle from './styles/search.module.scss';
import reviewStyle from "./styles/review.module.scss";
import paginationStyle from './styles/pagination.module.scss';
import reviewTabStyle from './styles/reviewTab.module.scss';
import {ReviewAccordion} from "./components/ReviewAccordion.jsx";
import {ReviewsPagination} from "./components/ReviewsPagination.jsx";
import {ReviewSortSelect} from "./components/ReviewSortSelect.jsx";
const PersonalReviewsTab = () => {
    // TODO: ajax запрос
    const reviews = [
        [
          {
            id: 1,
            isPositive: true,
            name: "Круто, я не смотрел",
            rating: 10,
            contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
            updatedAt: "22.8.2024",
            text: "Это тут насрали на кровать?"
          },
          {
            id: 2,
            isPositive: false,
            name: "Круто?",
            rating: 2,
            contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
            updatedAt: "22.8.2024",
            text: "aga aga aga aga"
          }
        ],
        [
          {
            id: 3,
            isPositive: true,
            name: "Занимайся жизнью или занимайся смертью",
            rating: 10,
            contentName: "Побег из Шоушенка",
            updatedAt: "1.1.2001",
            text: "xdd"
          }
        ]
    ];

    const [currentPage, setCurrentPage] = useState(1);
    const [sortType, setSortType] = useState("rating");
    
    function handleSearchSubmit(event) {
        event.preventDefault();
        const fd = new FormData(event.target);
        fd.append("sort", sortType);
        
        // TODO: запрос
    }

  return (
        <div className={reviewTabStyle.reviewTab}>
            <div className={searchStyle.searchFilterBox}>
              <form className={searchStyle.searchForm} onSubmit={handleSearchSubmit}>
                <div className={searchStyle.searchInputWrapper}>
                  <input className={searchStyle.searchInput} placeholder={"Поиск по слову"} type={"text"}
                          name={"search"}/>
                  <input type={"image"} src={searchIcon} width={45} height={45} alt={"Submit"}/>
                </div>
                <div className={searchStyle.sortSelectWrapper}>
                  <p className={searchStyle.sortLabel}>Сортировать по:</p>
                  <ReviewSortSelect setSortType={setSortType}/>
                </div>
              </form>
            </div>
            <div className={reviewStyle.reviewsBox}>
                {reviews[currentPage - 1].map(review => (
                  <ReviewAccordion key={review.id} review={review}></ReviewAccordion>
                ))}
            </div>
            <div className={paginationStyle.pagination}>
                <ReviewsPagination pageCount={reviews.length} currentPage={currentPage} setCurrentPage={setCurrentPage}/>
            </div>
        </div>
    )
}

export default PersonalReviewsTab