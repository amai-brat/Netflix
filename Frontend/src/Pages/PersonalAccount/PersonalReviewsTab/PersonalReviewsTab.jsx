import { useEffect, useState } from 'react';
import searchIcon from '../../../assets/Search.svg';
import searchStyle from './styles/search.module.scss';
import reviewStyle from "./styles/review.module.scss";
import paginationStyle from './styles/pagination.module.scss';
import reviewTabStyle from './styles/reviewTab.module.scss';
import {ReviewAccordion} from "./components/ReviewAccordion.jsx";
import {ReviewsPagination} from "./components/ReviewsPagination.jsx";
import {ReviewSortSelect} from "./components/ReviewSortSelect.jsx";
import {baseUrl} from '../../Shared/HttpClient/baseUrl.js';

const PersonalReviewsTab = () => {
  const reviews = [
      [
        {
          id: 1,
          isPositive: true,
          name: "Круто, я не смотрел",
          score: 10,
          contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
          writtenAt: "22.8.2024",
          text: "Это тут насрали на кровать?"
        },
        {
          id: 2,
          isPositive: false,
          name: "Круто?",
          score: 2,
          contentName: "Пираты карибского моря: Проклятие чёрной жемчужины",
          writtenAt: "22.8.2024",
          text: "aga aga aga aga"
        }
      ],
      [
        {
          id: 3,
          isPositive: true,
          name: "Занимайся жизнью или занимайся смертью",
          score: 10,
          contentName: "Побег из Шоушенка",
          writtenAt: "1.1.2001",
          text: "xdd"
        }
      ]
  ];

  const [currentPage, setCurrentPage] = useState(1);
  const [sortType, setSortType] = useState("rating");
  const [pageCount, setPageCount] = useState(1);
  const [pageReviews, setPageReviews] = useState([]);

  async function handleSearchSubmit(event) {
      event.preventDefault();
      const fd = new FormData(event.target);
      fd.append("sort", sortType);
      
      try {
        const response = await fetch(`${baseUrl}user/get-reviews-pages-count?input=${fd.get("search")}`);
        if (response.ok) {
          const count = await response.text();
          setPageCount(+count);
        }

        const responsee = await fetch(`${baseUrl}user/get-reviews?sort=${fd.get("sort")}&input=${fd.get("search")}&page=${currentPage - 1}`);
        if (responsee.ok) {
          const revs = await responsee.json();
          setPageReviews(revs);
        }
      } catch(error) {
        console.log(error);
      }
  }
  
  useEffect(() => {
    (async () => {
      const formData = new FormData(document.getElementById("search-form"));
      try {
        const response = await fetch(`${baseUrl}user/get-reviews-pages-count?input=${formData.get("search")}`);
        if (response.ok) {
          setPageCount(+(await response.text()));
        }
      } catch (error) {
        console.log(error);
      }
    })()
  }, [currentPage]);

  useEffect(() => {
    (async () => {
      const formData = new FormData(document.getElementById("search-form"));
      try {
        const response = await fetch(`${baseUrl}user/get-reviews?input=${formData.get("search")}&sort=${sortType}&page=${currentPage - 1}`);
        if (response.ok) {
          const revs = await response.json();
          setPageReviews(revs);
        }
      } catch (error) {
        console.log(error);
      }
    })()
  }, [currentPage])

  return (
        <div className={reviewTabStyle.reviewTab}>
            <div className={searchStyle.searchFilterBox}>
              <form className={searchStyle.searchForm} onSubmit={handleSearchSubmit} id={"search-form"}>
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
                {/* {reviews[currentPage - 1].map(review => (
                  <ReviewAccordion key={review.id} review={review}></ReviewAccordion>
                ))} */}
                {pageReviews.map(review => (
                  <ReviewAccordion key={review.id} review={review}></ReviewAccordion>
                ))}
            </div>
            <div className={paginationStyle.pagination}>
                <ReviewsPagination pageCount={pageCount} currentPage={currentPage} setCurrentPage={setCurrentPage}/>
            </div>
        </div>
    )
}

export default PersonalReviewsTab